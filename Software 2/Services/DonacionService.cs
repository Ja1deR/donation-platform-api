using Software_2.Models;
using Software_2.Repositories;

namespace Software_2.Services
{
    public class DonacionService
    {
        private readonly DonacionRepository _donacionRepo;

        public DonacionService(DonacionRepository donacionRepo)
        {
            _donacionRepo = donacionRepo;
        }

        public void CrearDonacion(Donacione donacion, int currentUserId)
        {
            donacion.FechaDonacion = DateTime.Now;
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
    }
}