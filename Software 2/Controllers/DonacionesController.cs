using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Software_2.Models;
using Software_2.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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

                // Validar que la publicación esté activa y no haya expirado
                var publicacion = _publicacionService.ObtenerPublicacion(donacionDTO.IdPublicacion);
                if (publicacion == null || !publicacion.Activa.GetValueOrDefault())
                    return BadRequest("La publicación no existe o está inactiva");

                if (publicacion.FechaFin.HasValue && publicacion.FechaFin < DateTime.Now)
                    return BadRequest("La publicación ha expirado");

                if (!publicacion.IdCategoriaDonacion.HasValue)
                    return BadRequest("La publicación no tiene una categoría válida");

                var donacion = new Donacione
                {
                    IdUsuarioDonante = currentUserId,
                    IdFundacion = publicacion.IdFundacion,
                    IdCategoriaDonacion = publicacion.IdCategoriaDonacion.Value,
                    Cantidad = donacionDTO.Cantidad,
                    DescripciónDonacion = donacionDTO.Descripcion,
                    FechaDonacion = DateTime.Now,
                    Ubicacion = donacionDTO.Ubicacion,
                    IdEstado = 1, // Estado inicial: Pendiente
                    IdPublicacion = donacionDTO.IdPublicacion
                };

                _donacionService.CrearDonacion(donacion, currentUserId);

                return Ok(new
                {
                    Mensaje = "Donación registrada exitosamente",
                    Donacion = donacion
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPatch("{id}/Estado")]
        [Authorize(Roles = "2,3")] // Solo administradores o fundaciones
        public IActionResult ActualizarEstado(int id, [FromBody] EstadoDTO estadoDTO)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _donacionService.ActualizarEstado(id, estadoDTO.IdEstado, currentUserId);
                return Ok(new { Mensaje = "Estado actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }

    // DTOs
    public class DonacionDTO
    {
        [Required]
        public int IdPublicacion { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        public string? Descripcion { get; set; }

        public string? Ubicacion { get; set; }
    }

    public class EstadoDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Estado inválido")]
        public int IdEstado { get; set; }
    }
}