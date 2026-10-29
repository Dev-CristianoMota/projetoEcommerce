namespace Ecommerce.DTO
{
    public class DTOProdutoMaisVendido
    {
        public int CdProduto { get; set; }
        public string NmProduto { get; set; } = string.Empty;
        public string NmLojaVirtual { get; set; } = "";
        public decimal VlProduto { get; set; }
        public int QtdVendida { get; set; }
        public string? NmUrlImagem { get; set; }
    }
}
