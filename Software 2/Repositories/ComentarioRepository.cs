using Microsoft.Data.SqlClient;
using Software_2.Models;
using System.Collections.Generic;
using System.Data;

namespace Software_2.Repositories
{
    public class ComentarioRepository
    {
        private readonly string _connectionString;

        public ComentarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ComentariosValoracione InsertarComentario(int idFundacion, int idUsuario, string comentario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("InsertComentarioSimple", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID_fundacion", idFundacion);
                command.Parameters.AddWithValue("@ID_usuario", idUsuario);
                command.Parameters.AddWithValue("@Comentario", comentario);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ComentariosValoracione
                        {
                            IdComentario = reader.GetInt32("ID_comentario"),
                            IdUsuario = reader.GetInt32("ID_usuario"),
                            IdFundacion = reader.GetInt32("ID_fundacion"),
                            Comentario = reader["Comentario"].ToString(),
                            FechaComentario = (DateTime)reader["Fecha_comentario"],
                            Aprobado = true
                        };
                    }
                }
            }
            return null; 
        }

        public List<ComentarioResponseDTO> ObtenerComentariosAprobados(int idFundacion)
        {
            var comentarios = new List<ComentarioResponseDTO>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("GetComentariosAprobadosPorFundacion", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID_fundacion", idFundacion);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comentarios.Add(new ComentarioResponseDTO
                        {
                            IdComentario = reader.GetInt32("ID_comentario"),
                            Comentario = reader["Comentario"].ToString(),
                            FechaComentario = (DateTime)reader["Fecha_comentario"],
                            UsuarioNombre = reader["Nombre_usuario"].ToString() 
                        });
                    }
                }
            }
            return comentarios; 
        }
    }
}