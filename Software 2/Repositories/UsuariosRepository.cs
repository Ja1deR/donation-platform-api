using Microsoft.Data.SqlClient;
using Software_2.Models;
using System.Collections.Generic;
using System.Data;

namespace Software_2.Repositories
{
    public class UsuariosRepository
    {
        private readonly string _connectionString;

        public UsuariosRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // RegistrarUsuario (SP: InsertUser)
        public void RegistrarUsuario(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("InsertUser", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Ajuste de nombres de propiedades
                command.Parameters.AddWithValue("@ID_rol", usuario.IdRol);
                command.Parameters.AddWithValue("@ID_tipo_documento", usuario.IdTipoDocumento);
                command.Parameters.AddWithValue("@Numero_documento", usuario.NumeroDocumento);
                command.Parameters.AddWithValue("@Nombre_usuario", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@Apellido_usuario", usuario.ApellidoUsuario);
                command.Parameters.AddWithValue("@Tel_usuario", usuario.TelUsuario ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Correo_usuario", usuario.CorreoUsuario);
                command.Parameters.AddWithValue("@Contraseña_usuario", ContraseñaHasher.Encrypt(usuario.ContraseñaUsuario));
                command.Parameters.AddWithValue("@Activo", usuario.Activo);

                // Obtener ID 
                usuario.IdUsuario = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        // ObtenerUsuario (SP: GetUser)
        public Usuario ObtenerUsuario(int id)
        {
            Usuario usuario = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID_usuario", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = reader.GetInt32("ID_usuario"),
                            IdRol = reader.GetInt32("ID_rol"),
                            IdTipoDocumento = reader.GetInt32("ID_tipo_documento"),
                            NumeroDocumento = reader.GetString("Numero_documento"),
                            NombreUsuario = reader.GetString("Nombre_usuario"),
                            ApellidoUsuario = reader.GetString("Apellido_usuario"),
                            TelUsuario = reader.IsDBNull("Tel_usuario") ? null : reader.GetString("Tel_usuario"),
                            CorreoUsuario = reader.GetString("Correo_usuario"),
                            ContraseñaUsuario = reader.GetString("Contraseña_usuario"),
                            FechaRegistro = reader.GetDateTime("Fecha_registro"),
                            Activo = reader.GetBoolean("Activo")
                        };
                    }
                }
            }
            return usuario;
        }

        // ListarUsuarios (SP: GetAllUsers)
        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetAllUsers", connection);
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = reader.GetInt32("ID_usuario"),
                            IdRol = reader.GetInt32("ID_rol"),
                            IdTipoDocumento = reader.GetInt32("ID_tipo_documento"),
                            NumeroDocumento = reader.GetString("Numero_documento"),
                            NombreUsuario = reader.GetString("Nombre_usuario"),
                            ApellidoUsuario = reader.GetString("Apellido_usuario"),
                            TelUsuario = reader.IsDBNull("Tel_usuario") ? null : reader.GetString("Tel_usuario"),
                            CorreoUsuario = reader.GetString("Correo_usuario"),
                            FechaRegistro = reader.GetDateTime("Fecha_registro"),
                            Activo = reader.GetBoolean("Activo")
                        });
                    }
                }
            }
            return usuarios;
        }

        // ModificarUsuario (SP: UpdateUser)
        public void ModificarUsuario(int id, Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UpdateUser", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID_usuario", id);
                command.Parameters.AddWithValue("@ID_rol", usuario.IdRol);
                command.Parameters.AddWithValue("@ID_tipo_documento", usuario.IdTipoDocumento);
                command.Parameters.AddWithValue("@Numero_documento", usuario.NumeroDocumento);
                command.Parameters.AddWithValue("@Nombre_usuario", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@Apellido_usuario", usuario.ApellidoUsuario);
                command.Parameters.AddWithValue("@Tel_usuario", usuario.TelUsuario ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Correo_usuario", usuario.CorreoUsuario);
                command.Parameters.AddWithValue("@Contraseña_usuario", ContraseñaHasher.Encrypt(usuario.ContraseñaUsuario));
                command.Parameters.AddWithValue("@Activo", usuario.Activo);

                command.ExecuteNonQuery();
            }
        }

        // EliminarUsuario (SP: DeleteUser)
        public void EliminarUsuario(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DeleteUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID_usuario", id);
                command.ExecuteNonQuery();
            }
        }
    }
}