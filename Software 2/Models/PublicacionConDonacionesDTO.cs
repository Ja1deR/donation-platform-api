namespace Software_2.Models;


public class PublicacionConDonacionesDTO
{
    public int IdPublicacion { get; set; }
    public string NombrePublicacion { get; set; }
    public string Descripcion { get; set; }      
    public DateTime FechaInicio { get; set; }    
    public DateTime? FechaFin { get; set; }   
    public int DonacionesRecibidas { get; set; }
    public int? MetaCantidad { get; set; }

    public decimal PorcentajeCumplimiento =>
        (MetaCantidad.HasValue && MetaCantidad > 0)
            ? Math.Round((decimal)DonacionesRecibidas / MetaCantidad.Value * 100, 2)
            : 0;
}