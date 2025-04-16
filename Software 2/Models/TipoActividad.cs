using System;
using System.Collections.Generic;

namespace Software_2.Models;

public partial class TipoActividad
{
    public int IdTipoActividad { get; set; }

    public string NombreTipoActividad { get; set; } = null!;

    public virtual ICollection<Fundación> Fundacións { get; set; } = new List<Fundación>();
}
