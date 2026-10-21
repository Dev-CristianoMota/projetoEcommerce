namespace Ecommerce.ViewModels
{
    public class ResumoPedidoViewModel
    {
        public int CdPedido { get; set; }
        public DateTime DtCriacao { get; set; }
        public string NmLoja { get; set; } = string.Empty;
        public string SgStatus { get; set; } = string.Empty;
        public decimal VlTotal { get; set; }
        public int QtItens { get; set; }
    }
}
