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
    public class FundacionController : ControllerBase
    {
        private readonly FundacionService _fundacionService;

        public FundacionController(FundacionService fundacionService)
        {
            _fundacionService = fundacionService;
        }

        [HttpPost("/CrearFundacion")]
        public IActionResult RegistrarFundacion([FromBody] FundacionDTO fundacionDTO)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); 

                var fundacion = new Fundación
                {
                    IdUsuario = currentUserId, 
                    NombreLegal = fundacionDTO.NombreLegal,
                    Nif = fundacionDTO.Nif,
                    Dirección = fundacionDTO.Direccion,
                    IdTipoActividad = fundacionDTO.IdTipoActividad,
                    Descripción = fundacionDTO.Descripcion,
                    SitioWeb = fundacionDTO.SitioWeb,
                    Activa = true,
                    FechaRegistro = DateTime.Now
                };

                _fundacionService.RegistrarFundacion(fundacion, currentUserId);
                return CreatedAtAction(nameof(ObtenerFundacion),
                    new { id = fundacion.IdFundacion },
                    new { Mensaje = "Fundación creada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("{id}/ObtenerUno")]
        public IActionResult ObtenerFundacion(int id)
        {
            var fundacion = _fundacionService.ObtenerFundacion(id);
            if (fundacion == null)
            {
                return NotFound(new { Error = $"Fundación con ID {id} no encontrada" });
            }
            return Ok(new
            {
                Mensaje = "Fundación encontrada",
                Fundacion = fundacion
            });
        }

        [HttpGet("/ObtenerTodas")]
        public IActionResult ListarFundaciones()
        {
            var fundaciones = _fundacionService.ListarFundaciones();
            return Ok(new
            {
                Total = fundaciones.Count,
                Fundaciones = fundaciones
            });
        }

        [HttpPut("{id}/Modificar")]
        public IActionResult ModificarFundacion(int id, [FromBody] FundacionUpdateDTO fundacionDTO)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); 

                var fundacionExistente = _fundacionService.ObtenerFundacion(id);
                if (fundacionExistente == null)
                    return NotFound(new { Error = $"Fundación con ID {id} no encontrada" });

                // Actualización condicional
                if (!string.IsNullOrWhiteSpace(fundacionDTO.NombreLegal))
                    fundacionExistente.NombreLegal = fundacionDTO.NombreLegal;

                if (!string.IsNullOrWhiteSpace(fundacionDTO.Nif))
                    fundacionExistente.Nif = fundacionDTO.Nif;

                if (!string.IsNullOrWhiteSpace(fundacionDTO.Direccion))
                    fundacionExistente.Dirección = fundacionDTO.Direccion;

                if (fundacionDTO.IdTipoActividad.HasValue)
                    fundacionExistente.IdTipoActividad = fundacionDTO.IdTipoActividad.Value;

                if (fundacionDTO.Descripcion != null)
                    fundacionExistente.Descripción = fundacionDTO.Descripcion;

                if (fundacionDTO.SitioWeb != null)
                    fundacionExistente.SitioWeb = fundacionDTO.SitioWeb;

                if (fundacionDTO.Activa.HasValue)
                    fundacionExistente.Activa = fundacionDTO.Activa.Value;

                _fundacionService.ModificarFundacion(id, fundacionExistente, currentUserId); 

                return Ok(new
                {
                    Mensaje = "Fundación actualizada exitosamente",
                    Id = id,
                    Fecha = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Error al actualizar fundación",
                    Detalle = ex.Message
                });
            }
        }

        [HttpDelete("{id}/Eliminar")]
        public IActionResult EliminarFundacion(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); 

                var fundacion = _fundacionService.ObtenerFundacion(id);
                if (fundacion == null)
                    return NotFound("Fundación no encontrada");

                fundacion.Activa = false;
                _fundacionService.ModificarFundacion(id, fundacion, currentUserId); 

                return Ok(new
                {
                    Mensaje = "Fundación desactivada exitosamente",
                    Id = id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Error al desactivar fundación",
                    Detalle = ex.Message
                });
            }
        }

        [HttpPatch("{id}/Reactivar")]
        public IActionResult ReactivarFundacion(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); 

                var fundacion = _fundacionService.ObtenerFundacion(id);
                if (fundacion == null)
                    return NotFound("Fundación no encontrada");

                fundacion.Activa = true;
                _fundacionService.ModificarFundacion(id, fundacion, currentUserId); 

                return Ok(new
                {
                    Mensaje = "Fundación reactivada correctamente",
                    Id = id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Error al reactivar fundación",
                    Detalle = ex.Message
                });
            }
        }
    }
}