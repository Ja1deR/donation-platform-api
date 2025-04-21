namespace Software_2.Models;

public partial class Donacione
{
    public int IdDonacion { get; set; }

    public int IdUsuarioDonante { get; set; }

    public int IdFundacion { get; set; }

    public int IdCategoriaDonacion { get; set; }

    public int? Cantidad { get; set; }

    public string? DescripciónDonacion { get; set; }

    public DateTime FechaDonacion { get; set; }

    public string? Ubicacion { get; set; }

    public int IdEstado { get; set; }

    public int? IdPublicacion { get; set; }

    public virtual ICollection<ComentariosValoracione> ComentariosValoraciones { get; set; } = new List<ComentariosValoracione>();

    public virtual CategoriaDonacion IdCategoriaDonacionNavigation { get; set; } = null!;

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Fundación IdFundacionNavigation { get; set; } = null!;

    public virtual Publicacione? IdPublicacionNavigation { get; set; }

    public virtual Usuario IdUsuarioDonanteNavigation { get; set; } = null!;

    public virtual ICollection<Notificacione> Notificaciones { get; set; } = new List<Notificacione>();
}
