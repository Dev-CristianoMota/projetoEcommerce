namespace Ecommerce.DTO
{
    public class DTOProdutoDetalhe
    {
        public int CdProduto { get; set; }
        public string NmProduto { get; set; } = "";
        public string? DsProduto { get; set; }
        public decimal VlProduto { get; set; }
        public int QtEstoque { get; set; }
        public string? NmUrlImagem { get; set; }
        public DateTime DtCriacao { get; set; }

        public int CdLojaVirtual { get; set; }
        public string NmLojaVirtual { get; set; } = "";

        public int CdCategoria { get; set; }
        public string NmCategoria { get; set; } = "";
    }
}
