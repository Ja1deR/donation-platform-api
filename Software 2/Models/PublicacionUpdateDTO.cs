using System.ComponentModel.DataAnnotations;

public class PublicacionUpdateDTO
{
    [StringLength(200)]
    public string? NombrePublicacion { get; set; }

    public string? Descripcion { get; set; }

    [Range(1, int.MaxValue)]
    public int? IdCategoriaDonacion { get; set; }

    public DateTime? FechaFin { get; set; }

    [Range(1, int.MaxValue)]
    public int? MetaCantidad { get; set; }

    public bool? Activa { get; set; }
}