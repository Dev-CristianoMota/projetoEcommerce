namespace Ecommerce.ViewModels
{
    public class CarrinhoItemViewModel
    {
        public int CdProduto { get; set; }
        public int CdLojaVirtual { get; set; }
        public string NmProduto { get; set; } = "";
        public string NmLojaVirtual { get; set; } = "";
        public string? NmUrlImagem { get; set; }
        public decimal VlProduto { get; set; }
        public int QtItem { get; set; }
    }
}
