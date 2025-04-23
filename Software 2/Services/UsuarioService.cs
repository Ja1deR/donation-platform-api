using Software_2.Repositories;
using Software_2.Models;
using Software_2.Helpers;

namespace Software_2.Services
{
    public class UsuarioService
    {
        private readonly UsuariosRepository _usuariosRepository;
        private readonly EmailTemplateService _emailTemplateService;
        private readonly EmailManager _emailManager;

        public UsuarioService(
            UsuariosRepository usuariosRepository,
            EmailTemplateService emailTemplateService,
            EmailManager emailManager)
        {
            _usuariosRepository = usuariosRepository;
            _emailTemplateService = emailTemplateService;
            _emailManager = emailManager;
        }

        public void RegistrarUsuario(Usuario usuario, int currentUserId)
        {
            _usuariosRepository.RegistrarUsuario(usuario, currentUserId);
        }

        public Usuario ObtenerUsuario(int id)
        {
            return _usuariosRepository.ObtenerUsuario(id);
        }

        public List<Usuario> ListarUsuarios()
        {
            return _usuariosRepository.ListarUsuarios();
        }

        public void ModificarUsuario(int id, Usuario usuario, int currentUserId)
        {
            _usuariosRepository.ModificarUsuario(id, usuario, currentUserId);
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