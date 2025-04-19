using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Software_2.Models;
using Software_2.Services;
using System.Collections.Generic;

namespace Software_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        [HttpPost("/Login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var usuario = _usuarioService.ObtenerUsuarioPorCorreo(loginDTO.CorreoUsuario);
                if (usuario == null)
                    return NotFound(new { Error = "Usuario no encontrado" });

                // Verificar contraseña
                var contraseñaEncriptada = ContraseñaHasher.Encrypt(loginDTO.Contraseña);
                if (usuario.ContraseñaUsuario != contraseñaEncriptada)
                    return Unauthorized(new { Error = "Contraseña incorrecta" });

                return Ok(new
                {
                    Mensaje = "Login exitoso",
                    Usuario = new
                    {
                        usuario.IdUsuario,
                        usuario.CorreoUsuario,
                        Rol = usuario.IdRol
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        // Controllers/UsuarioController.cs
        [HttpPost("/CrearUsuario")]
        public IActionResult RegistrarUsuario([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                // Mapear el DTO a la entidad Usuario
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

                _usuarioService.RegistrarUsuario(usuario);
                return CreatedAtAction(nameof(ObtenerUsuario), new { id = usuario.IdUsuario }, "Usuario registrado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Error al crear Usuario",
                    Detalle = ex.Message
                });
            }
           
        }

        [HttpGet("{id}/ObtenerUno")]
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
        public IActionResult ListarUsuarios()
        {
            List<Usuario> usuarios = _usuarioService.ListarUsuarios(); 
            return Ok(usuarios);
        }

        [HttpPut("{id}/Modificar")]
        public IActionResult ModificarUsuario(int id, [FromBody] UsuarioUpdateDTO usuarioDTO)
        {
            try
            {
                var usuarioExistente = _usuarioService.ObtenerUsuario(id);
                if (usuarioExistente == null)
                    return NotFound(new { Error = $"Usuario con ID {id} no encontrado" });

                // Mapeo condicional
                if (usuarioDTO.IdRol.HasValue) usuarioExistente.IdRol = usuarioDTO.IdRol.Value;
                if (usuarioDTO.IdTipoDocumento.HasValue) usuarioExistente.IdTipoDocumento = usuarioDTO.IdTipoDocumento.Value;
                if (!string.IsNullOrWhiteSpace(usuarioDTO.NumeroDocumento)) usuarioExistente.NumeroDocumento = usuarioDTO.NumeroDocumento;
                if (!string.IsNullOrWhiteSpace(usuarioDTO.NombreUsuario)) usuarioExistente.NombreUsuario = usuarioDTO.NombreUsuario;
                if (!string.IsNullOrWhiteSpace(usuarioDTO.ApellidoUsuario)) usuarioExistente.ApellidoUsuario = usuarioDTO.ApellidoUsuario;
                if (!string.IsNullOrWhiteSpace(usuarioDTO.TelUsuario)) usuarioExistente.TelUsuario = usuarioDTO.TelUsuario;
                if (!string.IsNullOrWhiteSpace(usuarioDTO.CorreoUsuario)) usuarioExistente.CorreoUsuario = usuarioDTO.CorreoUsuario;
                if (!string.IsNullOrWhiteSpace(usuarioDTO.ContraseñaUsuario))
                    usuarioExistente.ContraseñaUsuario = ContraseñaHasher.Encrypt(usuarioDTO.ContraseñaUsuario);
                if (usuarioDTO.Activo.HasValue) usuarioExistente.Activo = usuarioDTO.Activo.Value;

                // Validación mejorada
                TryValidateModel(usuarioExistente);

                // Remover errores de propiedades de navegación
                ModelState.Remove("IdRolNavigation");
                ModelState.Remove("IdTipoDocumentoNavigation");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _usuarioService.ModificarUsuario(id, usuarioExistente);

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
        public IActionResult EliminarUsuario(int id)
        {
            try
            {
                var usuario = _usuarioService.ObtenerUsuario(id);
                if (usuario == null)
                    return NotFound("Usuario no encontrado.");

                // Eliminación lógica
                usuario.Activo = false;
                _usuarioService.ModificarUsuario(id, usuario);

                return Ok("Usuario desactivado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPatch("{id}/Reactivar")]
        public IActionResult ReactivarUsuario(int id)
        {
            try
            {
                var usuario = _usuarioService.ObtenerUsuarioInactivo(id);
                if (usuario == null) return NotFound("Usuario no encontrado");

                usuario.Activo = true;
                _usuarioService.ModificarUsuario(id, usuario);

                return Ok("Usuario reactivado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}