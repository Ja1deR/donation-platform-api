using System.ComponentModel.DataAnnotations;

namespace Software_2.Models
{
 
    public class UsuarioDTO
    {
        [Required]
        public int IdRol { get; set; }

        [Required]
        public int IdTipoDocumento { get; set; }

        [Required]
        public string NumeroDocumento { get; set; }

        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        public string ApellidoUsuario { get; set; }

        public string? TelUsuario { get; set; }

        [Required]
        [EmailAddress]
        public string CorreoUsuario { get; set; }

        [Required]
        public string ContraseñaUsuario { get; set; }

        public bool Activo { get; set; } = true;
    }
}
