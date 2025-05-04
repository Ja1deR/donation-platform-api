using Software_2.Repositories;
using Software_2.Models;

namespace Software_2.Services
{
    public class FundacionService
    {
        private readonly FundacionRepository _fundacionRepository;

        public FundacionService(FundacionRepository fundacionRepository)
        {
            _fundacionRepository = fundacionRepository;
        }

        public void RegistrarFundacion(Fundación fundacion, int currentUserId)
        {
            fundacion.FechaRegistro = DateTime.Now;
            _fundacionRepository.RegistrarFundacion(fundacion, currentUserId);
        }

        public Fundación ObtenerFundacion(int id)
        {
            return _fundacionRepository.ObtenerFundacion(id);
        }

        public List<Fundación> ListarFundaciones()
        {
            return _fundacionRepository.ListarFundaciones();
        }

        public void ModificarFundacion(int id, Fundación fundacion, int currentUserId)
        {
            _fundacionRepository.ModificarFundacion(id, fundacion, currentUserId);
        }

        public void EliminarFundacion(int id, int currentUserId) 
        {
            var fundacion = _fundacionRepository.ObtenerFundacion(id);
            if (fundacion != null)
            {
                fundacion.Activa = false;
                _fundacionRepository.ModificarFundacion(id, fundacion, currentUserId); 
            }
        }
        public Fundación ObtenerFundacionPorUsuario(int idUsuario)
        {
            return _fundacionRepository.ObtenerFundacionPorUsuario(idUsuario);
        }

        public List<DonacionResponseDTO> ObtenerDonacionesHistoricas(
            int idFundacion,
            int pagina,
            int tamanoPagina,
            out int totalRegistros)
        {
            return _fundacionRepository.ObtenerDonacionesHistoricas(
                idFundacion,
                pagina,
                tamanoPagina,
                out totalRegistros
            );
        }
    }
}