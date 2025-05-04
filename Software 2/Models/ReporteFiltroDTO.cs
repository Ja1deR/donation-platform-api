using System.ComponentModel.DataAnnotations;

namespace Software_2.Models
{
 
    public class ReporteFiltroDTO
    {
        public int IdFundacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}