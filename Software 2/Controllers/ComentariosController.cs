using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Software_2.Models;
using Software_2.Models.Software_2.Models;
using Software_2.Services;
using System.Security.Claims;

namespace Software_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly ComentarioService _comentarioService;

        public ComentariosController(ComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }


        [HttpPost("fundacion/{idFundacion}")]
        [Authorize]
        public IActionResult CrearComentario(int idFundacion, [FromBody] ComentarioSimpleDTO dto)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _comentarioService.CrearComentario(idFundacion, currentUserId, dto.Comentario);
            return Ok(new { Mensaje = "Comentario publicado" });
        }


        [HttpGet("fundacion/{idFundacion}")]
        [AllowAnonymous]
        public IActionResult ObtenerComentarios(int idFundacion)
        {
            var comentarios = _comentarioService.ObtenerComentariosPorFundacion(idFundacion);
            return Ok(comentarios);
        }
    }
}