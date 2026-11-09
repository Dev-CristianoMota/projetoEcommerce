namespace Ecommerce.ViewModels
{
    public class DetalhePedidoViewModel : ResumoPedidoViewModel
    {
        public decimal VlSubtotal { get; set; }
        public decimal VlDesconto { get; set; }
        public DateTime? DtAtualizacao { get; set; }

        public string? CodigoCupom { get; set; }
        public string? TipoDesconto { get; set; }   // 'P' (percentual) ou 'F' (fixo)
        public decimal? VlCupom { get; set; }

        public EnderecoEntregaViewModel? Endereco { get; set; }
        public List<ItemPedidoViewModel> Itens { get; set; } = new();

        public IEnumerable<IGrouping<string, ItemPedidoViewModel>> ItensPorLoja
            => Itens.GroupBy(i => i.NmLoja);
    }
}