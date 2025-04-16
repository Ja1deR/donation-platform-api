using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        // Controllers/UsuarioController.cs
        [HttpPost]
        public IActionResult RegistrarUsuario([FromBody] UsuarioDTO usuarioDTO)
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

        [HttpGet("{id}")]
        public IActionResult ObtenerUsuario(int id)
        {
            var usuario = _usuarioService.ObtenerUsuario(id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            return Ok(usuario);
        }

        [HttpGet] 
        public IActionResult ListarUsuarios()
        {
            List<Usuario> usuarios = _usuarioService.ListarUsuarios(); 
            return Ok(usuarios);
        }

        [HttpPut("{id}")]
        public IActionResult ModificarUsuario(int id, [FromBody] Usuario usuario)
        {
            _usuarioService.ModificarUsuario(id, usuario);
            return Ok("Usuario modificado exitosamente.");
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            _usuarioService.EliminarUsuario(id);
            return Ok("Usuario eliminado exitosamente.");
        }
    }
}