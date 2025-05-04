using Software_2.Data;
using Software_2.Helpers;
using Software_2.Models;
using Microsoft.EntityFrameworkCore;
using Software_2.Repositories;

namespace Software_2.Services
{
    public class DonacionService
    {
        private readonly DonacionRepository _donacionRepo;
        private readonly EmailTemplateService _emailTemplateService;
        private readonly EmailManager _emailManager;
        private readonly AppDbContext _context;

        public DonacionService(
            DonacionRepository donacionRepo,
            EmailTemplateService emailTemplateService,
            EmailManager emailManager,
            AppDbContext context)
        {
            _donacionRepo = donacionRepo;
            _emailTemplateService = emailTemplateService;
            _emailManager = emailManager;
            _context = context;
        }


        public void CrearDonacion(Donacione donacion, int currentUserId)
        {
            donacion.FechaDonacion = DateTime.Now;
            donacion.IdEstado = 1; 
            _donacionRepo.CrearDonacion(donacion, currentUserId);
        }

        public List<DonacionResponseDTO> ObtenerDonacionesPorFundacion(int idFundacion)
        {
            return _donacionRepo.ObtenerDonacionesPorFundacion(idFundacion)
                .Select(d => new DonacionResponseDTO
                {
                    IdDonacion = d.IdDonacion,
                    NombreDonante = d.IdUsuarioDonanteNavigation.NombreUsuario,
                    Cantidad = d.Cantidad,
                    FechaDonacion = d.FechaDonacion,
                    Estado = d.IdEstadoNavigation.NombreEstado,
                    Publicacion = d.IdPublicacionNavigation.NombrePublicacion
                }).ToList();
        }
        public List<DonacionResponseDTO> ObtenerDonacionesParaReporte(
            int idFundacion,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            return _donacionRepo.ObtenerDonacionesParaReporte(
                idFundacion,
                fechaInicio,
                fechaFin
            );
        }
        public void ActualizarEstadoDonacion(int idDonacion, int idEstado, int currentUserId)
        {
            // Obtener datos de la donación
            var donacion = _context.Donaciones
                .Include(d => d.IdEstadoNavigation)
                .Include(d => d.IdUsuarioDonanteNavigation)
                .Include(d => d.IdFundacionNavigation)
                .FirstOrDefault(d => d.IdDonacion == idDonacion);

            if (donacion == null) throw new Exception("Donación no encontrada");

            var estadoAnterior = donacion.IdEstadoNavigation.NombreEstado;

            _donacionRepo.ActualizarEstadoDonacion(idDonacion, idEstado, currentUserId);

            // Obtener nuevo estado
            var nuevoEstado = _context.Estados.First(e => e.IdEstado == idEstado).NombreEstado;

            // Enviar correo
            string htmlContent = _emailTemplateService.ConstruirMensajeCambioEstado(
                donacion.IdUsuarioDonanteNavigation.NombreUsuario,
                donacion.Cantidad,
                donacion.IdFundacionNavigation.NombreLegal,
                estadoAnterior,
                nuevoEstado
            );

            _emailManager.EnviarCorreo(
                donacion.IdUsuarioDonanteNavigation.CorreoUsuario,
                "Estado de tu donación actualizado",
                htmlContent,
                true
            );
        }

        public int ObtenerTotalDonacionesPorFundacion(int idFundacion)
        {
            return _donacionRepo.ObtenerTotalDonacionesPorFundacion(idFundacion);
        }
    }
}