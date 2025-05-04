using Microsoft.Data.SqlClient;
using Software_2.Models;
using System.Data;

namespace Software_2.Repositories
{
    public class DonacionRepository
    {
        private readonly string _connectionString;

        public DonacionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CrearDonacion(Donacione donacion, int currentUserId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("InsertDonacion", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID_usuario_donante", donacion.IdUsuarioDonante);
                command.Parameters.AddWithValue("@ID_fundacion", donacion.IdFundacion);
                command.Parameters.AddWithValue("@ID_categoria_donacion", donacion.IdCategoriaDonacion);
                command.Parameters.AddWithValue("@Cantidad", donacion.Cantidad);
                command.Parameters.AddWithValue("@Descripcion_donacion", donacion.DescripciónDonacion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Ubicacion", donacion.Ubicacion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ID_estado", donacion.IdEstado);
                command.Parameters.AddWithValue("@ID_publicacion", donacion.IdPublicacion);
                command.Parameters.AddWithValue("@CurrentUserID", currentUserId);

                command.ExecuteNonQuery();
            }
        }

        public List<Donacione> ObtenerDonacionesPorFundacion(int idFundacion)
        {
            var donaciones = new List<Donacione>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("GetDonacionesPorFundacion", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ID_fundacion", idFundacion);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        donaciones.Add(new Donacione
                        {
                            IdDonacion = reader.GetInt32("ID_donacion"),
                            Cantidad = reader.GetInt32("Cantidad"),
                            FechaDonacion = reader.GetDateTime("Fecha_donacion"),
                            IdEstadoNavigation = new Estado
                            {
                                NombreEstado = reader.GetString("Nombre_estado")
                            },
                            IdUsuarioDonanteNavigation = new Usuario
                            {
                                NombreUsuario = reader.GetString("Nombre_usuario")
                            },
                            IdPublicacionNavigation = new Publicacione
                            {
                                NombrePublicacion = reader.GetString("Nombre_publicacion")
                            }
                        });
                    }
                }
            }
            return donaciones;
        }
    }
}