using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Software_2.Models;
using Software_2.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Software_2.Data;
using Software_2.Helpers;

namespace Software_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context; 
        private readonly EmailTemplateService _emailTemplateService;
        private readonly EmailManager _emailManager;


        public UsuarioController(
            UsuarioService usuarioService,
            IConfiguration config,
            AppDbContext context,
            EmailTemplateService emailTemplateService,
            EmailManager emailManager)
        {
            _usuarioService = usuarioService;
            _config = config;
            _context = context; 
            _emailTemplateService = emailTemplateService;
            _emailManager = emailManager;
        }



        [HttpPost("/Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var usuario = _usuarioService.ObtenerUsuarioPorCorreo(loginDTO.CorreoUsuario);
                if (usuario == null)
                    return Unauthorized(new { Error = "Credenciales inválidas" });

                string contraseñaEncriptada = ContraseñaHasher.Encrypt(loginDTO.Contraseña);
                if (usuario.ContraseñaUsuario != contraseñaEncriptada)
                    return Unauthorized(new { Error = "Credenciales inválidas" });

                // Generar Access Token
                var accessToken = GenerateJwtToken(usuario);

                // Generar Refresh Token
                var refreshToken = new RefreshToken
                {
                    Token = GenerateRefreshToken(),
                    Expires = DateTime.UtcNow.AddDays(_config.GetValue<int>("Jwt:RefreshTokenExpireDays")),
                    IdUsuario = usuario.IdUsuario
                };

                // Guardar en BD
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.RefreshTokens.Add(refreshToken);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                // ========== ENVÍO DE CORREO ==========
                string htmlContent = _emailTemplateService.ConstruirMensajeLogin(usuario.NombreUsuario);
                _emailManager.EnviarCorreo(
                    usuario.CorreoUsuario,
                    "Inicio de sesión exitoso - Rescate Solidario",
                    htmlContent,
                    true
                );
                // ======================================

                return Ok(new
                {
                    AccessToken = accessToken.TokenString,
                    RefreshToken = refreshToken.Token,
                    ExpiraEn = accessToken.Expires,
                    Usuario = new { usuario.IdUsuario, usuario.CorreoUsuario, usuario.IdRol }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }


        private (string TokenString, DateTime Expires) GenerateJwtToken(Usuario usuario)
        {
          
            var rol = _context.Roles.FirstOrDefault(r => r.IdRol == usuario.IdRol);
            var nombreRol = rol?.NombreRol ?? "Donante"; 

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var expires = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:AccessTokenExpireMinutes"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Email, usuario.CorreoUsuario),
                    new Claim(ClaimTypes.Role, nombreRol)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), expires);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public class RefreshTokenRequest
        {
            public string RefreshToken { get; set; } = null!;
        }

        [HttpPost("/refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var refreshToken = await _context.RefreshTokens
                    .Include(rt => rt.Usuario)
                    .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

                if (refreshToken == null || refreshToken.IsExpired)
                    return Unauthorized(new { Error = "Refresh token inválido o expirado" });


                var accessToken = GenerateJwtToken(refreshToken.Usuario);

                _context.RefreshTokens.Remove(refreshToken);
                var newRefreshToken = new RefreshToken
                {
                    Token = GenerateRefreshToken(),
                    Expires = DateTime.UtcNow.AddDays(_config.GetValue<int>("Jwt:RefreshTokenExpireDays")),
                    IdUsuario = refreshToken.IdUsuario
                };
                _context.RefreshTokens.Add(newRefreshToken);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    AccessToken = accessToken.TokenString,
                    RefreshToken = newRefreshToken.Token,
                    ExpiraEn = accessToken.Expires
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("/CrearUsuario")]
        [AllowAnonymous]
        public IActionResult RegistrarUsuario([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                int systemUserId = 0;

                var usuario = new Usuario
                {
                    IdRol = usuarioDTO.IdRol,
                    IdTipoDocumento = usuarioDTO.IdTipoDocumento,
                    NumeroDocumento = usuarioDTO.NumeroDocumento,
                    NombreUsuario = usuarioDTO.NombreUsuario,
                    ApellidoUsuario = usuarioDTO.ApellidoUsuario,
                    TelUsuario = usuarioDTO.TelUsuario,
                    CorreoUsuario = usuarioDTO.CorreoUsuario,
                    ContraseñaUsuario = ContraseñaHasher.Encrypt(usuarioDTO.ContraseñaUsuario),
                    Activo = usuarioDTO.Activo
                };

                _usuarioService.RegistrarUsuario(usuario, systemUserId);

                return CreatedAtAction(nameof(ObtenerUsuario),
                    new { id = usuario.IdUsuario },
                    new { Mensaje = "Usuario registrado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Error al crear Usuario", Detalle = ex.Message });
            }
        }

        [HttpGet("{id}/ObtenerUno")]
        [Authorize(Roles = "Administrador")]
        public IActionResult ObtenerUsuario(int id)
        {
            var usuario = _usuarioService.ObtenerUsuario(id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            return Ok(usuario);
        }

        [HttpGet("/Obtener todos")]
        [Authorize(Roles = "Administrador")]
        public IActionResult ListarUsuarios()
        {
            List<Usuario> usuarios = _usuarioService.ListarUsuarios();
            return Ok(usuarios);
        }

        [HttpPut("{id}/Modificar")]
        [Authorize(Roles = "Administrador")]
        public IActionResult ModificarUsuario(int id, [FromBody] UsuarioUpdateDTO usuarioDTO)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var usuarioExistente = _usuarioService.ObtenerUsuario(id);
                if (usuarioExistente == null)
                    return NotFound(new { Error = $"Usuario con ID {id} no encontrado" });


                if (usuarioDTO.IdRol.HasValue)
                    usuarioExistente.IdRol = usuarioDTO.IdRol.Value;

                if (usuarioDTO.IdTipoDocumento.HasValue)
                    usuarioExistente.IdTipoDocumento = usuarioDTO.IdTipoDocumento.Value;

                if (!string.IsNullOrWhiteSpace(usuarioDTO.NumeroDocumento))
                    usuarioExistente.NumeroDocumento = usuarioDTO.NumeroDocumento;

                if (!string.IsNullOrWhiteSpace(usuarioDTO.NombreUsuario))
                    usuarioExistente.NombreUsuario = usuarioDTO.NombreUsuario;

                if (!string.IsNullOrWhiteSpace(usuarioDTO.ApellidoUsuario))
                    usuarioExistente.ApellidoUsuario = usuarioDTO.ApellidoUsuario;

                if (!string.IsNullOrWhiteSpace(usuarioDTO.TelUsuario))
                    usuarioExistente.TelUsuario = usuarioDTO.TelUsuario;

                if (!string.IsNullOrWhiteSpace(usuarioDTO.CorreoUsuario))
                    usuarioExistente.CorreoUsuario = usuarioDTO.CorreoUsuario;

                if (!string.IsNullOrWhiteSpace(usuarioDTO.ContraseñaUsuario))
                    usuarioExistente.ContraseñaUsuario = ContraseñaHasher.Encrypt(usuarioDTO.ContraseñaUsuario);

                if (usuarioDTO.Activo.HasValue)
                    usuarioExistente.Activo = usuarioDTO.Activo.Value;

                TryValidateModel(usuarioExistente);


                ModelState.Remove("IdRolNavigation");
                ModelState.Remove("IdTipoDocumentoNavigation");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _usuarioService.ModificarUsuario(id, usuarioExistente, currentUserId);

                return Ok(new
                {
                    Mensaje = "Usuario actualizado exitosamente",
                    Id = id,
                    Fecha = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Error interno del servidor",
                    Detalle = ex.Message
                });
            }
        }

        [HttpDelete("{id}/Eliminar(desactivar)")]
        [Authorize(Roles = "Administrador")]
        public IActionResult EliminarUsuario(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var usuario = _usuarioService.ObtenerUsuario(id);
                if (usuario == null)
                    return NotFound("Usuario no encontrado.");


                usuario.Activo = false;
                _usuarioService.ModificarUsuario(id, usuario, currentUserId);

                return Ok(new
                {
                    Mensaje = "Usuario desactivado exitosamente",
                    Id = id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Error al desactivar usuario",
                    Detalle = ex.Message
                });
            }
        }

        [HttpPatch("{id}/Reactivar")]
        [Authorize(Roles = "Administrador")]
        public IActionResult ReactivarUsuario(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var usuario = _usuarioService.ObtenerUsuarioInactivo(id);
                if (usuario == null)
                    return NotFound("Usuario no encontrado");

                usuario.Activo = true;
                _usuarioService.ModificarUsuario(id, usuario, currentUserId);

                return Ok(new
                {
                    Mensaje = "Usuario reactivado correctamente",
                    Id = id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Error al reactivar usuario",
                    Detalle = ex.Message
                });
            }
        }

        [HttpPut("{id}/ActualizarRol")]
        [Authorize(Roles = "Administrador")] 
        public IActionResult ActualizarRol(int id, [FromBody] ActualizarRolDTO dto)
        {
            try
            {
                var usuario = _context.Usuarios.Find(id);
                if (usuario == null)
                    return NotFound("Usuario no encontrado.");

               
                var rolExiste = _context.Roles.Any(r => r.IdRol == dto.NuevoIdRol);
                if (!rolExiste)
                    return BadRequest("El rol especificado no existe.");

                usuario.IdRol = dto.NuevoIdRol;
                _context.SaveChanges();

                return Ok(new { Mensaje = "Rol actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }


        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var tokens = await _context.RefreshTokens
                .Where(rt => rt.IdUsuario == userId)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();

            return Ok(new { Mensaje = "Logout exitoso" });
        }
    }
}