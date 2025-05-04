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
        private readonly PublicacionService _publicacionService;
        private readonly DonacionService _donacionService; 

        
        public FundacionController(
            FundacionService fundacionService,
            PublicacionService publicacionService, 
            DonacionService donacionService) 
        {
            _fundacionService = fundacionService;
            _publicacionService = publicacionService; 
            _donacionService = donacionService; 
        }

        [HttpPost("/CrearFundacion")]
        [Authorize(Roles = "Fundacion, Administrador")]
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
        [AllowAnonymous]

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
        [AllowAnonymous]
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
        [Authorize(Roles = "Fundacion, Administrador")]
        public IActionResult ModificarFundacion(int id, [FromBody] FundacionUpdateDTO fundacionDTO)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); 

                var fundacionExistente = _fundacionService.ObtenerFundacion(id);
                if (fundacionExistente == null)
                    return NotFound(new { Error = $"Fundación con ID {id} no encontrada" });

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
        [Authorize(Roles = "Fundacion, Administrador")]
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
        [Authorize(Roles = "Fundacion, Administrador")]
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
      
        [HttpGet("{id}/donaciones")]
        [AllowAnonymous]
        public IActionResult ObtenerDonacionesPorFundacion(
            int id,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 10)
        {
            try
            {
                int totalRegistros;
                var donaciones = _fundacionService.ObtenerDonacionesHistoricas(
                    id,
                    pagina,
                    tamanoPagina,
                    out totalRegistros
                );

                return Ok(new
                {
                    Pagina = pagina,
                    TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanoPagina),
                    TotalRegistros = totalRegistros,
                    Donaciones = donaciones
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
  
        [HttpGet("{id}/perfil")]
        [AllowAnonymous] 
        public IActionResult ObtenerPerfilFundacion(int id)
        {
            try
            {
                var fundacion = _fundacionService.ObtenerFundacion(id);
                if (fundacion == null || !fundacion.Activa.GetValueOrDefault())
                    return NotFound("Fundación no encontrada o inactiva");

                var perfil = new PerfilFundacionDTO
                {
                    IdFundacion = fundacion.IdFundacion,
                    NombreLegal = fundacion.NombreLegal,
                    Descripcion = fundacion.Descripción ?? "Sin descripción",
                    SitioWeb = fundacion.SitioWeb,
                    FechaRegistro = fundacion.FechaRegistro,
                    PublicacionesActivas = _publicacionService.ObtenerPublicacionesActivasConProgreso(id),
                    TotalDonacionesHistoricas = _donacionService.ObtenerTotalDonacionesPorFundacion(id)
                };

                return Ok(perfil);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

    }
}