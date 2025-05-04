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
                command.Parameters.AddWithValue("@Descripción_donacion", donacion.DescripciónDonacion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Ubicacion", donacion.Ubicacion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ID_estado", donacion.IdEstado);
                command.Parameters.AddWithValue("@ID_publicacion", donacion.IdPublicacion);
                command.Parameters.AddWithValue("@CurrentUserID", currentUserId);

                command.ExecuteNonQuery();

            }

        }

        public void ActualizarEstado(int idDonacion, int idEstado, int currentUserId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UpdateEstadoDonacion", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID_donacion", idDonacion);
                command.Parameters.AddWithValue("@ID_estado", idEstado);
                command.Parameters.AddWithValue("@CurrentUserID", currentUserId);

                command.ExecuteNonQuery();
            }
        }
    }
}