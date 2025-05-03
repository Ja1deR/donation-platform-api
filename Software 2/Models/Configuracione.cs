namespace Software_2.Models;

public partial class Configuracione
{
    public int IdConfiguracion { get; set; }

    public string Clave { get; set; } = null!;

    public string Valor { get; set; } = null!;

    public string? Descripción { get; set; }
}
