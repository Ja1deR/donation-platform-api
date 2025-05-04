using System.ComponentModel.DataAnnotations;

namespace Software_2.Models
{
    public class DonacionDTO
    {
        [Required(ErrorMessage = "La publicación es requerida")]
        public int IdPublicacion { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string? DescripcionDonacion { get; set; }

        [StringLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string? Ubicacion { get; set; }
    }
}