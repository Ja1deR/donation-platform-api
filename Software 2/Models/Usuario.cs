using System;
using System.Collections.Generic;

namespace Software_2.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public int IdTipoDocumento { get; set; }

    public string NumeroDocumento { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public string ApellidoUsuario { get; set; } = null!;

    public string? TelUsuario { get; set; }

    public string CorreoUsuario { get; set; } = null!;

    public string ContraseñaUsuario { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public DateTime? UltimoAcceso { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Auditorium> Auditoria { get; set; } = new List<Auditorium>();

    public virtual ICollection<ComentariosValoracione> ComentariosValoraciones { get; set; } = new List<ComentariosValoracione>();

    public virtual ICollection<Donacione> Donaciones { get; set; } = new List<Donacione>();

    public virtual ICollection<Fundación> Fundacións { get; set; } = new List<Fundación>();

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual TipoDocumento IdTipoDocumentoNavigation { get; set; } = null!;

    public virtual ICollection<Notificacione> Notificaciones { get; set; } = new List<Notificacione>();
}
