using System.ComponentModel.DataAnnotations;

namespace Software_2.Models
{
    public class PublicacionDTO
    {
        [Required(ErrorMessage = "El nombre de la publicación es requerido")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string NombrePublicacion { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La categoría de donación es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoría inválida")]
        public int IdCategoriaDonacion { get; set; }

        public DateTime? FechaFin { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La meta debe ser mayor a 0")]
        public int? MetaCantidad { get; set; }
    }
}