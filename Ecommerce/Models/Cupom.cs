using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class Cupom
{
    public int CdCupom { get; set; }

    public string CdPromocional { get; set; } = null!;

    public string SgTipoDesconto { get; set; } = null!;

    public decimal VlDesconto { get; set; }

    public DateTime DtInicioValidade { get; set; }

    public DateTime DtFimValidade { get; set; }

    public bool? IcCupomAtivo { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
