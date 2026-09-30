using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class ItemPedido
{
    public int CdItemPedido { get; set; }

    public int CdPedido { get; set; }

    public int CdProduto { get; set; }

    public int QtItem { get; set; }

    public decimal VlPrecoUnitario { get; set; }

    public virtual Pedido CdPedidoNavigation { get; set; } = null!;

    public virtual Produto CdProdutoNavigation { get; set; } = null!;
}
