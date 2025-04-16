using System;
using System.Collections.Generic;

namespace Software_2.Models;

public partial class ComentariosValoracione
{
    public int IdComentario { get; set; }

    public int IdUsuario { get; set; }

    public int IdFundacion { get; set; }

    public int? IdDonacion { get; set; }

    public string? Comentario { get; set; }

    public int? Valoracion { get; set; }

    public DateTime FechaComentario { get; set; }

    public string? Respuesta { get; set; }

    public bool? Aprobado { get; set; }

    public bool? Reportado { get; set; }

    public virtual Donacione? IdDonacionNavigation { get; set; }

    public virtual Fundación IdFundacionNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
