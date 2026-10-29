using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class Produto
{
    public int CdProduto { get; set; }

    public int CdLojaVirtual { get; set; }

    public int CdCategoria { get; set; }

    public string NmProduto { get; set; } = null!;

    public string? DsProduto { get; set; }

    public decimal VlProduto { get; set; }

    public int QtdEstoque { get; set; }

    public bool? IcProdutoAtivo { get; set; }

    public DateTime DtCriacao { get; set; }

    public DateTime? DtAtualizacao { get; set; }

    public string? NmUrlImagem { get; set; }

    public string? CdPublicIdImagem { get; set; }

    public virtual Categoria CdCategoriaNavigation { get; set; } = null!;

    public virtual LojaVirtual CdLojaVirtualNavigation { get; set; } = null!;

    public virtual ICollection<ItemPedido> ItemPedidos { get; set; } = new List<ItemPedido>();
}
