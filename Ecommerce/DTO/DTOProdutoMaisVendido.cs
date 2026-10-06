namespace Ecommerce.DTO
{
    public class DTOProdutoMaisVendido
    {
        public int CdProduto { get; set; }
        public string NmProduto { get; set; } = string.Empty;
        public decimal VlProduto { get; set; }
        public int QtdVendida { get; set; }
    }
}
