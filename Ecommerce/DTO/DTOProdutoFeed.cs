namespace Ecommerce.DTO
{
    public class DTOProdutoFeed
    {
        public int CdProduto { get; set; }
        public string NmProduto { get; set; } = string.Empty;
        public string NmLojaVirtual { get; set; } = string.Empty;
        public string NmCategoria { get; set; } = string.Empty;
        public decimal VlProduto { get; set; }
        public int QtEstoque { get; set; }
        public string? NmUrlImagem { get; set; }
        public DateTime DtCriacao { get; set; }
    }
}
