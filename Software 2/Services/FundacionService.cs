using Software_2.Repositories;
using Software_2.Models;
using System.Collections.Generic;

namespace Software_2.Services
{
    public class FundacionService
    {
        private readonly FundacionRepository _fundacionRepository;

        public FundacionService(FundacionRepository fundacionRepository)
        {
            _fundacionRepository = fundacionRepository;
        }

        public void RegistrarFundacion(Fundación fundacion)
        {
            fundacion.FechaRegistro = DateTime.Now; 
            _fundacionRepository.RegistrarFundacion(fundacion);
        }

        public Fundación ObtenerFundacion(int id)
        {
            return _fundacionRepository.ObtenerFundacion(id);
        }

        public List<Fundación> ListarFundaciones()
        {
            return _fundacionRepository.ListarFundaciones();
        }

        public void ModificarFundacion(int id, Fundación fundacion)
        {
            _fundacionRepository.ModificarFundacion(id, fundacion);
        }

        public void EliminarFundacion(int id)
        {
            var fundacion = _fundacionRepository.ObtenerFundacion(id);
            if (fundacion != null)
            {
                fundacion.Activa = false;
                _fundacionRepository.ModificarFundacion(id, fundacion);
            }
        }
    }
}