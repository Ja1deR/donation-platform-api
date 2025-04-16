using System;
using System.Collections.Generic;

namespace Software_2.Models;

public partial class Fundación
{
    public int IdFundacion { get; set; }

    public int IdUsuario { get; set; }

    public string NombreLegal { get; set; } = null!;

    public string Nif { get; set; } = null!;

    public string Dirección { get; set; } = null!;

    public int IdTipoActividad { get; set; }

    public string? Descripción { get; set; }

    public string? SitioWeb { get; set; }

    public DateTime FechaRegistro { get; set; }

    public bool? Activa { get; set; }

    public virtual ICollection<ComentariosValoracione> ComentariosValoraciones { get; set; } = new List<ComentariosValoracione>();

    public virtual ICollection<Donacione> Donaciones { get; set; } = new List<Donacione>();

    public virtual ICollection<FundacionDocumento> FundacionDocumentos { get; set; } = new List<FundacionDocumento>();

    public virtual TipoActividad IdTipoActividadNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Publicacione> Publicaciones { get; set; } = new List<Publicacione>();
}
