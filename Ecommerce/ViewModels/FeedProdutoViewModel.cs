using Ecommerce.DTO;

namespace Ecommerce.ViewModels
{
    public class FiltroProdutos
    {
        public string Ordem { get; set; } = "recentes";

        public List<int> Categorias { get; set; } = new();

        public List<int> Lojas { get; set; } = new();

        public int? PrecoMin { get; set; }
        public int? PrecoMax { get; set; }

        public bool ApenasEstoque { get; set; }

        public string? Busca { get; set; }
    }

    public record ResultadoFeed(List<DTOProdutoFeed> Itens, int? Total, bool TemMais);

    public class FeedProdutosViewModel
    {
        public FiltroProdutos Filtro { get; set; } = new();
        public List<DTOProdutoFeed> Produtos { get; set; } = new();
        public List<DTOCategoriaFiltro> Categorias { get; set; } = new();
        public List<DTOLojaFiltro> Lojas { get; set; } = new();
        public int Total { get; set; }
        public bool TemMais { get; set; }
    }
}
