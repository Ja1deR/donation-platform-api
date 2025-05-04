using Software_2.Models;
using Software_2.Repositories;

namespace Software_2.Services
{
    public class DonacionService
    {
        private readonly DonacionRepository _donacionRepository;

        public DonacionService(DonacionRepository donacionRepository)
        {
            _donacionRepository = donacionRepository;
        }

        public void CrearDonacion(Donacione donacion, int currentUserId)
        {
            _donacionRepository.CrearDonacion(donacion, currentUserId);
        }

        public void ActualizarEstado(int idDonacion, int idEstado, int currentUserId)
        {
            _donacionRepository.ActualizarEstado(idDonacion, idEstado, currentUserId);
        }

    }
}