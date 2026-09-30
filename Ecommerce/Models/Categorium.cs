using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class Categorium
{
    public int CdCategoria { get; set; }

    public string NmCategoria { get; set; } = null!;

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
