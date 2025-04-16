using System;
using System.Collections.Generic;

namespace Software_2.Models;

public partial class Auditorium
{
    public int IdAuditoria { get; set; }

    public int IdUsuario { get; set; }

    public string Accion { get; set; } = null!;

    public DateTime FechaAccion { get; set; }

    public string? TablaAfectada { get; set; }

    public int? IdRegistroAfectado { get; set; }

    public string? Detalles { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
