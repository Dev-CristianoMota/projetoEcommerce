using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class LojaVirtual
{
    public int CdLojaVirtual { get; set; }

    public int CdUsuario { get; set; }

    public string NmLojaVirtual { get; set; } = null!;

    public string? DsLojaVirtual { get; set; }

    public bool? IcLojaAtiva { get; set; }

    public DateTime DtCriacao { get; set; }

    public virtual Usuario CdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
