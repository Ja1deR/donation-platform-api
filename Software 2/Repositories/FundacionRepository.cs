using Microsoft.Data.SqlClient;
using Software_2.Models;
using System.Data;

namespace Software_2.Repositories
{
    public class FundacionRepository
    {
        private readonly string _connectionString;

        public FundacionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void RegistrarFundacion(Fundación fundacion, int currentUserId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("InsertFundacion", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID_usuario", fundacion.IdUsuario);
                command.Parameters.AddWithValue("@Nombre_legal", fundacion.NombreLegal);
                command.Parameters.AddWithValue("@NIF", fundacion.Nif);
                command.Parameters.AddWithValue("@Dirección", fundacion.Dirección);
                command.Parameters.AddWithValue("@ID_tipo_actividad", fundacion.IdTipoActividad);
                command.Parameters.AddWithValue("@Descripción", fundacion.Descripción ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Sitio_web", fundacion.SitioWeb ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Activa", fundacion.Activa);
                command.Parameters.AddWithValue("@CurrentUserID", currentUserId);

                fundacion.IdFundacion = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public Fundación ObtenerFundacion(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("GetFundacion", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ID_fundacion", id);

                using (var reader = command.ExecuteReader())
                {
                    return reader.Read() ? MapearFundacion(reader) : null;
                }
            }
        }

        public List<Fundación> ListarFundaciones()
        {
            var fundaciones = new List<Fundación>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("GetAllFundaciones", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fundaciones.Add(MapearFundacion(reader));
                    }
                }
            }
            return fundaciones;
        }

        public void ModificarFundacion(int id, Fundación fundacion, int currentUserId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UpdateFundacion", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID_fundacion", id);
                command.Parameters.AddWithValue("@Nombre_legal", fundacion.NombreLegal);
                command.Parameters.AddWithValue("@NIF", fundacion.Nif);
                command.Parameters.AddWithValue("@Dirección", fundacion.Dirección);
                command.Parameters.AddWithValue("@ID_tipo_actividad", fundacion.IdTipoActividad);
                command.Parameters.AddWithValue("@Descripción", fundacion.Descripción ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Sitio_web", fundacion.SitioWeb ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Activa", fundacion.Activa);
                command.Parameters.AddWithValue("@CurrentUserID", currentUserId);

                command.ExecuteNonQuery();
            }
        }

        public Fundación ObtenerFundacionPorUsuario(int idUsuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("GetFundacionPorUsuario", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ID_usuario", idUsuario);

                using (var reader = command.ExecuteReader())
                {
                    return reader.Read() ? MapearFundacion(reader) : null;
                }
            }
        }
        private Fundación MapearFundacion(SqlDataReader reader)
        {
            return new Fundación
            {
                IdFundacion = reader.GetInt32(reader.GetOrdinal("ID_fundacion")),
                IdUsuario = reader.GetInt32(reader.GetOrdinal("ID_usuario")),
                NombreLegal = reader.GetString(reader.GetOrdinal("Nombre_legal")),
                Nif = reader.GetString(reader.GetOrdinal("NIF")),
                Dirección = reader.GetString(reader.GetOrdinal("Dirección")),
                IdTipoActividad = reader.GetInt32(reader.GetOrdinal("ID_tipo_actividad")),
                Descripción = reader.IsDBNull("Descripción") ? null : reader.GetString("Descripción"),
                SitioWeb = reader.IsDBNull("Sitio_web") ? null : reader.GetString("Sitio_web"),
                FechaRegistro = reader.GetDateTime("Fecha_registro"),
                Activa = reader.GetBoolean("Activa")
            };
        }
    }
}