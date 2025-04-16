using System;
using System.Collections.Generic;

namespace Software_2.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string NombreEstado { get; set; } = null!;

    public virtual ICollection<Donacione> Donaciones { get; set; } = new List<Donacione>();
}
