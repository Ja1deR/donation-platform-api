using Software_2.Models;
using Software_2.Repositories;
using System.Collections.Generic;

namespace Software_2.Services
{
    public class ComentarioService
    {
        private readonly ComentarioRepository _repo;

        public ComentarioService(ComentarioRepository repo)
        {
            _repo = repo;
        }

        public void CrearComentario(int idFundacion, int idUsuario, string comentario)
        {
            _repo.InsertarComentario(idFundacion, idUsuario, comentario);
        }

        public List<ComentarioResponseDTO> ObtenerComentariosPorFundacion(int idFundacion)
        {
            return _repo.ObtenerComentariosAprobados(idFundacion); 
        }
    }
}