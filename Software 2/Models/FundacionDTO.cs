using System.ComponentModel.DataAnnotations;

namespace Software_2.Models
{
    public class FundacionDTO
    {
        [Required(ErrorMessage = "El ID de usuario es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de usuario inválido")]
        public int IdUsuario { get; set; } 

        [Required(ErrorMessage = "Nombre legal es requerido")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string NombreLegal { get; set; } = null!;

        [Required(ErrorMessage = "NIF es requerido")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Nif { get; set; } = null!;

        [Required(ErrorMessage = "Dirección es requerida")]
        [StringLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "Tipo de actividad es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de tipo de actividad inválido")]
        public int IdTipoActividad { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string? Descripcion { get; set; }

        [Url(ErrorMessage = "URL inválida")]
        [StringLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string? SitioWeb { get; set; }

        public bool Activa { get; set; } = true;
    }
}