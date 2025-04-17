using System.ComponentModel.DataAnnotations;

namespace Software_2.Models
{
    public class FundacionUpdateDTO
    {
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Nombre legal debe tener entre 2 y 200 caracteres")]
        public string? NombreLegal { get; set; }

        [RegularExpression(@"^[A-Z0-9]{5,50}$",
            ErrorMessage = "NIF debe contener solo mayúsculas y números (5-50 caracteres)")]
        public string? Nif { get; set; }

        [StringLength(255, MinimumLength = 10,
            ErrorMessage = "Dirección debe tener entre 10 y 255 caracteres")]
        public string? Direccion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo de actividad válido")]
        public int? IdTipoActividad { get; set; }

        [StringLength(500, ErrorMessage = "Descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Url(ErrorMessage = "Formato de URL inválido (ej: https://tusitio.com)")]
        [StringLength(255, ErrorMessage = "URL no puede exceder 255 caracteres")]
        public string? SitioWeb { get; set; }

        public bool? Activa { get; set; }
    }
}