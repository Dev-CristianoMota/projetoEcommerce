namespace Ecommerce.DTO
{
    public class UltimoPedidoDTO
    {
        public int CdPedido { get; set; }
        public int CdUsuario { get; set; }

        public DateTime DtCriacaoPedido { get; set; }
        public string SgStatusPedido { get; set; } = string.Empty;

        public int CdLojaVirtual { get; set; }
        public string NmLojaVirtual { get; set; } = string.Empty;

        public decimal VlSubtotalPedido { get; set; }
        public decimal VlDescontoPedido { get; set; }
        public decimal VlTotalPedido { get; set; }

        public string? CdPromocional { get; set; }

        public int? CdProduto { get; set; }
        public string? NmProduto { get; set; }
        public int? QtItem { get; set; }
        public decimal? VlPrecoUnitario { get; set; }
        public decimal? VlTotalItem { get; set; }
    }
}
