using System.ComponentModel.DataAnnotations;

namespace Software_2.Models
{
    public class DonacionResponseDTO
    {
        public int IdDonacion { get; set; }
        public string NombreDonante { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaDonacion { get; set; }
        public string Estado { get; set; }
        public string Publicacion { get; set; }
    }
}