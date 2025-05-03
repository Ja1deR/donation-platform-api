namespace Software_2.Models;

public partial class CategoriaDonacion
{
    public int IdCategoriaDonacion { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public string? Descripción { get; set; }

    public bool? Activa { get; set; }

    public virtual ICollection<Donacione> Donaciones { get; set; } = new List<Donacione>();

    public virtual ICollection<Publicacione> Publicaciones { get; set; } = new List<Publicacione>();
}
