using System.ComponentModel.DataAnnotations.Schema;

namespace Software_2.Models
{
    public class PerfilFundacionDTO
    {
        public int IdFundacion { get; set; }
        public string NombreLegal { get; set; }
        public string Descripcion { get; set; }
        public string SitioWeb { get; set; }
        public List<PublicacionConDonacionesDTO> PublicacionesActivas { get; set; }
        public int TotalDonacionesHistoricas { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class PublicacionConDonacionesDTO
    {
        public int IdPublicacion { get; set; }
        public string NombrePublicacion { get; set; }
        public int DonacionesRecibidas { get; set; }
        public int? MetaCantidad { get; set; }
        public decimal PorcentajeCumplimiento => MetaCantidad.HasValue && MetaCantidad > 0 ?
            (decimal)DonacionesRecibidas / MetaCantidad.Value * 100 : 0;
    }
}