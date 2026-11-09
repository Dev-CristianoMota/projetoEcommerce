using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class Pedido
{
    public int CdPedido { get; set; }

    public int CdUsuario { get; set; }

    public int? CdCupom { get; set; }

    public string SgStatusPedido { get; set; } = null!;

    public decimal VlSubtotalPedido { get; set; }

    public decimal VlDescontoPedido { get; set; }

    public decimal VlTotalPedido { get; set; }

    public DateTime DtCriacaoPedido { get; set; }

    public DateTime? DtAtualizacaoPedido { get; set; }

    public virtual Cupom? CdCupomNavigation { get; set; }

    public virtual Usuario CdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<ItemPedido> ItemPedidos { get; set; } = new List<ItemPedido>();

    public int? CdEnderecoEntrega { get; set; }

    public virtual Endereco? CdEnderecoEntregaNavigation { get; set; }
}
