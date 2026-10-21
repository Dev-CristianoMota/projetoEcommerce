namespace Ecommerce.ViewModels
{
    public class MeusPedidosViewModel
    {
        public List<ResumoPedidoViewModel> Pedidos { get; set; } = new();
        public string? StatusFiltro { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
        public string Ordem { get; set; } = "recentes";
    }
}
