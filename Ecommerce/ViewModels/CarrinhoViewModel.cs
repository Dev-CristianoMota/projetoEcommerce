namespace Ecommerce.ViewModels
{
    public class CarrinhoViewModel
    {
        public bool UsuarioLogado { get; set; }
        public string UrlLogin { get; set; } = "";
        public string? Mensagem { get; set; }
        public List<CarrinhoItemViewModel> Itens { get; set; } = new();

        public int? CdCupom { get; set; }
        public string? CodigoCupom { get; set; }
        public string? DescricaoCupom { get; set; }
        public decimal Desconto { get; set; }
        public string? ErroCupom { get; set; }

        public decimal Subtotal => Itens.Sum(i => i.VlProduto * i.QtItem);
        public int QuantidadeTotal => Itens.Sum(i => i.QtItem);
        public bool CupomAplicado => !string.IsNullOrEmpty(CodigoCupom);
        public decimal Total => Math.Max(Subtotal - Desconto, 0);
    }
}