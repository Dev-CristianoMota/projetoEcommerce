namespace Ecommerce.ViewModels
{
    public class CheckoutViewModel
    {
        public CarrinhoViewModel Carrinho { get; set; } = new();
        public List<CheckoutLojaViewModel> Lojas { get; set; } = new();
        public List<CheckoutEnderecoViewModel> Enderecos { get; set; } = new();
        public int? CdEnderecoSelecionado { get; set; }
        public string? Erro { get; set; }
    }

    public class CheckoutLojaViewModel
    {
        public int CdLojaVirtual { get; set; }
        public string NmLojaVirtual { get; set; } = "";
        public List<CarrinhoItemViewModel> Itens { get; set; } = new();
        public decimal Subtotal { get; set; }
    }

    public class CheckoutEnderecoViewModel
    {
        public int CdEndereco { get; set; }
        public string NmIdentificacao { get; set; } = "";
        public string NmDestinatario { get; set; } = "";
        public string NmCep { get; set; } = "";
        public string NmLogradouro { get; set; } = "";
        public string NmNumero { get; set; } = "";
        public string? DsComplemento { get; set; }
        public string NmBairro { get; set; } = "";
        public string NmCidade { get; set; } = "";
        public string SgUf { get; set; } = "";
        public bool IcPrincipal { get; set; }

        public string Linha1 => $"{NmLogradouro}, {NmNumero}"
            + (string.IsNullOrWhiteSpace(DsComplemento) ? "" : $" — {DsComplemento}");

        public string Linha2 => $"{NmBairro} · {NmCidade}/{SgUf} · CEP {CepFormatado}";

        private string CepFormatado => NmCep.Length == 8 ? $"{NmCep[..5]}-{NmCep[5..]}" : NmCep;
    }

    public class ConfirmacaoPedidoViewModel
    {
        public int CdPedido { get; set; }
        public DateTime DtCriacao { get; set; }
        public int QtItens { get; set; }
        public decimal VlSubtotal { get; set; }
        public decimal VlDesconto { get; set; }
        public decimal VlTotal { get; set; }
        public string? CodigoCupom { get; set; }
        public CheckoutEnderecoViewModel? Endereco { get; set; }
        public List<CheckoutLojaViewModel> Lojas { get; set; } = new();
    }
}