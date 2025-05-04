using System.Security.Claims;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Software_2.Helpers;
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
        private readonly UsuarioService _usuarioService;
        private readonly FundacionService _fundacionService;
        private readonly EmailTemplateService _emailTemplateService; 
        private readonly EmailManager _emailManager; 

        public DonacionesController(
            DonacionService donacionService,
            PublicacionService publicacionService,
            UsuarioService usuarioService,
            FundacionService fundacionService,
            EmailTemplateService emailTemplateService,
            EmailManager emailManager) 
        {
            _donacionService = donacionService;
            _publicacionService = publicacionService;
            _usuarioService = usuarioService;
            _fundacionService = fundacionService;
            _emailTemplateService = emailTemplateService;
            _emailManager = emailManager;
        }

        [HttpPost]
        [Authorize(Roles = "Fundacion, Administrador, Donante")]
        public IActionResult CrearDonacion([FromBody] DonacionDTO donacionDTO)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Validar publicación
                var publicacion = _publicacionService.ObtenerPublicacion(donacionDTO.IdPublicacion);
                if (publicacion == null || !publicacion.Activa.GetValueOrDefault())
                    return BadRequest("Publicación no válida");
                if (!publicacion.IdCategoriaDonacion.HasValue)
                    return BadRequest("La publicación no tiene una categoría válida");

                // Crear donación (IdPublicacion es obligatorio por el DTO)
                var donacion = new Donacione
                {
                    IdUsuarioDonante = currentUserId,
                    IdFundacion = publicacion.IdFundacion,
                    IdPublicacion = donacionDTO.IdPublicacion, // ✅ Siempre tiene valor
                    IdCategoriaDonacion = publicacion.IdCategoriaDonacion.Value,
                    Cantidad = donacionDTO.Cantidad,
                    DescripciónDonacion = donacionDTO.DescripcionDonacion,
                    Ubicacion = donacionDTO.Ubicacion,
                    IdEstado = 1
                };

                _donacionService.CrearDonacion(donacion, currentUserId);

                // Obtener datos para el email (IdPublicacion no es nulo aquí)
                var usuarioDonante = _usuarioService.ObtenerUsuario(currentUserId);
                var fundacion = _fundacionService.ObtenerFundacion(donacion.IdFundacion);
                var publicacionDonacion = _publicacionService.ObtenerPublicacion(donacion.IdPublicacion!.Value);

                // Construir y enviar email
                string htmlContent = _emailTemplateService.ConstruirMensajeDonacion(
                    usuarioDonante.NombreUsuario,
                    donacion.Cantidad,
                    fundacion.NombreLegal,
                    publicacionDonacion?.NombrePublicacion ?? "Necesidad General"
                );

                _emailManager.EnviarCorreo(
                    usuarioDonante.CorreoUsuario,
                    "¡Gracias por tu donación!",
                    htmlContent,
                    true
                );

                return Ok(new { Mensaje = "Donación registrada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }


        [HttpGet("fundacion/{idFundacion}")]
        [AllowAnonymous]
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
        [HttpPut("{id}/Estado")]
        [Authorize(Roles = "Administrador")]
        public IActionResult ActualizarEstadoDonacion(int id, [FromBody] ActualizarEstadoDonacionDTO dto)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _donacionService.ActualizarEstadoDonacion(id, dto.IdEstado, currentUserId);
                return Ok(new { Mensaje = "Estado de la donación actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
        
        [HttpPost("GenerarReporteExcel")]
        [AllowAnonymous]
        public IActionResult GenerarReporteExcel([FromBody] ReporteFiltroDTO filtro)
        {
            try
            {
                var donaciones = _donacionService.ObtenerDonacionesParaReporte(
                    filtro.IdFundacion,
                    filtro.FechaInicio,
                    filtro.FechaFin
                );

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Donaciones");

                    // Cabeceras
                    worksheet.Cell(1, 1).Value = "Donante";
                    worksheet.Cell(1, 2).Value = "Cantidad";
                    worksheet.Cell(1, 3).Value = "Fecha";
                    worksheet.Cell(1, 4).Value = "Estado";
                    worksheet.Cell(1, 5).Value = "Publicación";

                    // Datos
                    int row = 2;
                    foreach (var d in donaciones)
                    {
                        worksheet.Cell(row, 1).Value = d.NombreDonante;
                        worksheet.Cell(row, 2).Value = d.Cantidad;
                        worksheet.Cell(row, 3).Value = d.FechaDonacion.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 4).Value = d.Estado;
                        worksheet.Cell(row, 5).Value = d.Publicacion;
                        row++;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            $"ReporteDonaciones_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}