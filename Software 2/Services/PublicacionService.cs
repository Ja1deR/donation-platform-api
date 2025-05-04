using Software_2.Models;
using Software_2.Repositories;

namespace Software_2.Services
{
    public class PublicacionService
    {
        private readonly PublicacionRepository _publicacionRepo;
        private readonly FundacionService _fundacionService;

        public PublicacionService(
            PublicacionRepository publicacionRepo,
            FundacionService fundacionService)
        {
            _publicacionRepo = publicacionRepo;
            _fundacionService = fundacionService;
        }

        public int CrearPublicacion(Publicacione publicacion, int currentUserId)
        {
            // Validar que la fundación está activa
            var fundacion = _fundacionService.ObtenerFundacion(publicacion.IdFundacion);
            if (fundacion == null || !fundacion.Activa.HasValue || !fundacion.Activa.Value)
                throw new Exception("Fundación no activa o no existe");

            publicacion.FechaInicio = DateTime.Now;
            publicacion.Activa = true;

            return _publicacionRepo.CrearPublicacion(publicacion, currentUserId);
        }

        public (List<Publicacione> Publicaciones, int TotalPaginas) ListarPublicaciones(int pagina, int tamanoPagina, int? categoriaId)
        {
            int totalRegistros;
            var publicaciones = _publicacionRepo.ListarPublicaciones(pagina, tamanoPagina, categoriaId, out totalRegistros);
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);
            return (publicaciones, totalPaginas);
        }
        public Publicacione ObtenerPublicacion(int id)
        {
            return _publicacionRepo.ObtenerPublicacion(id);
        }

        // Opcional: Para progreso de donaciones
        public decimal? ObtenerProgresoDonacion(int idPublicacion)
        {
            return _publicacionRepo.ObtenerProgresoDonacion(idPublicacion);
        }
    }
}