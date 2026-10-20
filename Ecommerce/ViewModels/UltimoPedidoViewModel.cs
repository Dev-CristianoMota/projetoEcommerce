
namespace Ecommerce.ViewModels
{
    public class ContaViewModel
    {
        public UltimoPedidoContaViewModel? UltimoPedido { get; set; }
    }

    public class UltimoPedidoContaViewModel
    {
        public int CdPedido { get; set; }

        public string NumeroFormatado => $"#{CdPedido:D6}";

        public DateTime DtCriacao { get; set; }

        public string SgStatus { get; set; } = "P";

        public string NmLoja { get; set; } = string.Empty;

        public string? CdPromocional { get; set; }

        public decimal VlSubtotal { get; set; }

        public decimal VlDesconto { get; set; }

        public decimal VlTotal { get; set; }

        public List<ItemUltimoPedidoViewModel> Itens { get; set; } = new();

        public int QtTotalItens => Itens.Sum(i => i.Quantidade);

        public string StatusTexto => SgStatus switch
        {
            "P" => "Pendente",
            "A" => "Aprovado",
            "E" => "Em transporte",
            "T" => "Entregue",
            "C" => "Cancelado",
            _ => "Desconhecido"
        };

        public string StatusCss => SgStatus switch
        {
            "E" => "status-transit",
            "T" => "status-delivered",
            "C" => "status-canceled",
            "A" => "status-approved",
            _ => "status-pending"
        };
    }

    public class ItemUltimoPedidoViewModel
    {
        public string NmProduto { get; set; } = string.Empty;

        public int Quantidade { get; set; }

        public decimal VlUnitario { get; set; }
    }
}