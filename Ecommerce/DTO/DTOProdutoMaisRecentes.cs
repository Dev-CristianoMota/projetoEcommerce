namespace Ecommerce.DTO
{
    public class DTOProdutoMaisRecentes
    {
        public int CdProduto { get; set; }
        public string NmProduto { get; set; } = "";
        public string NmLojaVirtual { get; set; } = "";
        public decimal VlProduto { get; set; }
        public DateTime DtCriacao { get; set; }
    }
}