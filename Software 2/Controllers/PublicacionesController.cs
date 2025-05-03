using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Software_2.Models;
using Software_2.Services;

namespace Software_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PublicacionesController : ControllerBase
    {
        private readonly PublicacionService _publicacionService;
        private readonly FundacionService _fundacionService;

        public PublicacionesController(
            PublicacionService publicacionService,
            FundacionService fundacionService)
        {
            _publicacionService = publicacionService;
            _fundacionService = fundacionService;
        }

        [HttpPost]
        public IActionResult CrearPublicacion([FromBody] PublicacionDTO publicacionDTO)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var fundacionUsuario = _fundacionService.ObtenerFundacionPorUsuario(currentUserId);
                if (fundacionUsuario == null || !fundacionUsuario.Activa.GetValueOrDefault())
                    return BadRequest("El usuario no tiene una fundación activa asociada");

                var publicacion = new Publicacione
                {
                    IdFundacion = fundacionUsuario.IdFundacion,
                    NombrePublicacion = publicacionDTO.NombrePublicacion,
                    Descripción = publicacionDTO.Descripcion,
                    FechaInicio = DateTime.Now,
                    FechaFin = publicacionDTO.FechaFin,
                    MetaCantidad = publicacionDTO.MetaCantidad,
                    IdCategoriaDonacion = publicacionDTO.IdCategoriaDonacion,
                    Activa = true
                };

                var idPublicacion = _publicacionService.CrearPublicacion(publicacion, currentUserId);

                // Obtener la publicación creada para retornarla
                var publicacionCreada = _publicacionService.ObtenerPublicacion(idPublicacion);

                return CreatedAtAction(
                    nameof(ObtenerPublicacion),
                    new { id = idPublicacion },
                    publicacionCreada // Agregar este parámetro
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListarPublicaciones(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 10,
            [FromQuery] int? categoriaId = null)
        {
            try
            {
                var publicaciones = _publicacionService.ListarPublicaciones(pagina, tamanoPagina, categoriaId);
                return Ok(new
                {
                    Pagina = pagina,
                    TotalPaginas = (int)Math.Ceiling(publicaciones.Count / (double)tamanoPagina),
                    Publicaciones = publicaciones
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult ObtenerPublicacion(int id)
        {
            try
            {
                var publicacion = _publicacionService.ObtenerPublicacion(id);
                if (publicacion == null)
                    return NotFound("Publicación no encontrada");

                var progreso = _publicacionService.ObtenerProgresoDonacion(id);

                return Ok(new
                {
                    Publicacion = publicacion,
                    Progreso = progreso
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

    }
}