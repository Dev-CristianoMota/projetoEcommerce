namespace Ecommerce.ViewModels
{
    public class ItemPedidoViewModel
    {
        public int cdProduto { get; set; }
        public int cdCategoria { get; set; }
        public string NmProduto { get; set; } = string.Empty;
        public string NmCategoria { get; set; } = string.Empty;
        public string NmLoja { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public string? NmUrlImagem { get; set; }
        public decimal VlUnitario { get; set; }
        public decimal VlSubtotalItem => Quantidade * VlUnitario;
    }
}
