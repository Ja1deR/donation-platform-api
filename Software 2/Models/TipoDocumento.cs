using System;
using System.Collections.Generic;

namespace Software_2.Models;

public partial class TipoDocumento
{
    public int IdTipoDocumento { get; set; }

    public string NombreTipoDocumento { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
