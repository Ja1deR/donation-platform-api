namespace Software_2.Models;

public partial class Notificacione
{
    public int IdNotificacion { get; set; }

    public int IdUsuario { get; set; }

    public int? IdDonacion { get; set; }

    public string TipoNotificacion { get; set; } = null!;

    public string Mensaje { get; set; } = null!;

    public DateTime FechaEnvio { get; set; }

    public bool? Enviada { get; set; }

    public virtual Donacione? IdDonacionNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
