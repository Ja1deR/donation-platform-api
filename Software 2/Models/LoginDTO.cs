using System.ComponentModel.DataAnnotations;

namespace Software_2.Models
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string CorreoUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Contraseña { get; set; }
    }
}