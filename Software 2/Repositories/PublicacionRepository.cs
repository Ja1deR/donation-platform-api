using Microsoft.Data.SqlClient;
using Software_2.Models;
using System.Data;

namespace Software_2.Repositories
{
    public class PublicacionRepository
    {
        private readonly string _connectionString;

        public PublicacionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int CrearPublicacion(Publicacione publicacion, int currentUserId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("InsertPublicacion", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@ID_fundacion", publicacion.IdFundacion);
                command.Parameters.AddWithValue("@Nombre_publicacion", publicacion.NombrePublicacion);
                command.Parameters.AddWithValue("@Descripcion", publicacion.Descripción);
                command.Parameters.AddWithValue("@Fecha_inicio", publicacion.FechaInicio);
                command.Parameters.AddWithValue("@Fecha_fin", publicacion.FechaFin ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Meta_cantidad", publicacion.MetaCantidad ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ID_categoria_donacion", publicacion.IdCategoriaDonacion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CurrentUserID", currentUserId);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public List<Publicacione> ListarPublicaciones(int pagina, int tamanoPagina, int? categoriaId, out int totalRegistros)
        {
            totalRegistros = 0;
            var publicaciones = new List<Publicacione>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("GetAllPublicaciones", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Pagina", pagina);
                command.Parameters.AddWithValue("@TamanoPagina", tamanoPagina);
                command.Parameters.AddWithValue("@ID_categoria", categoriaId ?? (object)DBNull.Value);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        publicaciones.Add(new Publicacione
                        {
                            IdPublicacion = reader.GetInt32("ID_publicacion"),
                            IdFundacion = reader.GetInt32("ID_fundacion"),
                            NombrePublicacion = reader.GetString("Nombre_publicacion"),
                            Descripción = reader.GetString("Descripción"),
                            FechaInicio = reader.GetDateTime("Fecha_inicio"),
                            FechaFin = reader.IsDBNull("Fecha_fin") ? null : reader.GetDateTime("Fecha_fin"),
                            MetaCantidad = reader.IsDBNull("Meta_cantidad") ? null : reader.GetInt32("Meta_cantidad"),
                            IdCategoriaDonacion = reader.IsDBNull("ID_categoria_donacion") ? null : reader.GetInt32("ID_categoria_donacion")
                        });
                    }

                    // Leer el total de registros si hay un segundo resultado (si se usa un DataSet)
                    if (reader.NextResult() && reader.Read())
                    {
                        totalRegistros = reader.GetInt32("TotalRegistros");
                    }
                }
            }
            return publicaciones;
        }
        public Publicacione? ObtenerPublicacion(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("GetPublicacion", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ID_publicacion", id);

                using (var reader = command.ExecuteReader())
                {
                    return reader.Read() ? new Publicacione
                    {
                        IdPublicacion = reader.GetInt32("ID_publicacion"),
                        IdFundacion = reader.GetInt32("ID_fundacion"),
                        NombrePublicacion = reader.GetString("Nombre_publicacion"),
                        Descripción = reader.GetString("Descripción"),
                        FechaInicio = reader.GetDateTime("Fecha_inicio"),
                        FechaFin = reader.IsDBNull("Fecha_fin") ? null : reader.GetDateTime("Fecha_fin"),
                        MetaCantidad = reader.IsDBNull("Meta_cantidad") ? null : reader.GetInt32("Meta_cantidad"),
                        IdCategoriaDonacion = reader.IsDBNull("ID_categoria_donacion") ? null : reader.GetInt32("ID_categoria_donacion"),
                        Activa = reader.GetBoolean("Activa")
                    } : null;
                }
            }
        }
        public decimal? ObtenerProgresoDonacion(int idPublicacion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("GetProgresoDonacion", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ID_publicacion", idPublicacion);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.IsDBNull("Progreso")
                            ? null
                            : reader.GetDecimal(reader.GetOrdinal("Progreso"));
                    }
                    return null;
                }
            }
        }

    }
}