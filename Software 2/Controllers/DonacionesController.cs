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
    public class DonacionesController : ControllerBase
    {
        private readonly DonacionService _donacionService;
        private readonly PublicacionService _publicacionService;

        public DonacionesController(
            DonacionService donacionService,
            PublicacionService publicacionService)
        {
            _donacionService = donacionService;
            _publicacionService = publicacionService;
        }

        [HttpPost]
        public IActionResult CrearDonacion([FromBody] DonacionDTO donacionDTO)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var publicacion = _publicacionService.ObtenerPublicacion(donacionDTO.IdPublicacion);
                if (publicacion == null || !publicacion.Activa.GetValueOrDefault())
                    return BadRequest("Publicación no válida");
                if (!publicacion.IdCategoriaDonacion.HasValue)
                    return BadRequest("La publicación no tiene una categoría válida");

                var donacion = new Donacione
                {
                    IdUsuarioDonante = currentUserId,
                    IdFundacion = publicacion.IdFundacion,
                    IdPublicacion = donacionDTO.IdPublicacion,
                    IdCategoriaDonacion = publicacion.IdCategoriaDonacion.Value,
                    Cantidad = donacionDTO.Cantidad,
                    DescripciónDonacion = donacionDTO.DescripcionDonacion,
                    Ubicacion = donacionDTO.Ubicacion,
                    IdEstado = 1 // Estado inicial (Ej: 'Pendiente')
                };

                _donacionService.CrearDonacion(donacion, currentUserId);

                return Ok(new { Mensaje = "Donación registrada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("fundacion/{idFundacion}")]
        public IActionResult ObtenerDonacionesPorFundacion(int idFundacion)
        {
            try
            {
                var donaciones = _donacionService.ObtenerDonacionesPorFundacion(idFundacion);
                return Ok(donaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}