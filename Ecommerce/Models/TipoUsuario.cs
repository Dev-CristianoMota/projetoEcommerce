using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class TipoUsuario
{
    public int CdTipoUsuario { get; set; }

    public string NmTipoUsuario { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
