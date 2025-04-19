
using Software_2.Repositories;
using Software_2.Models;
using System.Collections.Generic;

namespace Software_2.Services
{
    public class UsuarioService
    {
        private readonly UsuariosRepository _usuariosRepository;


        public UsuarioService(UsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        public void RegistrarUsuario(Usuario usuario)
        {
            _usuariosRepository.RegistrarUsuario(usuario);
        }

        public Usuario ObtenerUsuario(int id)
        {
            return _usuariosRepository.ObtenerUsuario(id);
        }

        public List<Usuario> ListarUsuarios()
        {
            return _usuariosRepository.ListarUsuarios();
        }

        public void ModificarUsuario(int id, Usuario usuario)
        {
            _usuariosRepository.ModificarUsuario(id, usuario);
        }

        public void EliminarUsuario(int id)
        {
            _usuariosRepository.EliminarUsuario(id);
        }
        public Usuario ObtenerUsuarioInactivo(int id)
        {
            return _usuariosRepository.ObtenerUsuarioIncluidoInactivo(id);
        }
        public Usuario ObtenerUsuarioPorCorreo(string correo)
        {
            return _usuariosRepository.ObtenerUsuarioPorCorreo(correo);
        }
    }
}