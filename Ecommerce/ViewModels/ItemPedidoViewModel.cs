namespace Ecommerce.ViewModels
{
    public class ItemPedidoViewModel
    {
        public string NmProduto { get; set; } = string.Empty;
        public string NmCategoria { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal VlUnitario { get; set; }
        public decimal VlSubtotalItem => Quantidade * VlUnitario;
    }
}
