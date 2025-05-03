namespace Software_2.Models;

public partial class Publicacione
{
    public int IdPublicacion { get; set; }

    public int IdFundacion { get; set; }

    public string NombrePublicacion { get; set; } = null!;

    public string Descripción { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? MetaCantidad { get; set; }

    public int? IdCategoriaDonacion { get; set; }

    public bool? Activa { get; set; }

    public virtual ICollection<Donacione> Donaciones { get; set; } = new List<Donacione>();

    public virtual CategoriaDonacion? IdCategoriaDonacionNavigation { get; set; }

    public virtual Fundación IdFundacionNavigation { get; set; } = null!;
}
