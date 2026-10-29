using Ecommerce.Data;
using Ecommerce.DTO;
using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services;

public class ProdutoFeedService
{
    public const int TamanhoPagina = 50;

    private readonly EcommerceDbContext _context;

    public ProdutoFeedService(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<ResultadoFeed> BuscarAsync(FiltroProdutos filtro, int pagina)
    {
        pagina = Math.Max(pagina, 1);

        var query = AplicarFiltros(filtro);

        // O total só é necessário na primeira carga (nas demais o JS já o tem)
        int? total = pagina == 1 ? await query.CountAsync() : null;

        // Busca 51 para saber se existe próxima página sem fazer outro COUNT
        var itens = await Ordenar(query, filtro.Ordem)
            .Skip((pagina - 1) * TamanhoPagina)
            .Take(TamanhoPagina + 1)
            .Select(p => new DTOProdutoFeed
            {
                CdProduto = p.CdProduto,
                NmProduto = p.NmProduto,
                NmLojaVirtual = p.CdLojaVirtualNavigation.NmLojaVirtual,
                NmCategoria = p.CdCategoriaNavigation.NmCategoria,
                VlProduto = p.VlProduto,
                QtEstoque = p.QtdEstoque,
                DtCriacao = p.DtCriacao,

                // >>> ADAPTE ESTA LINHA <<<
                // Use a mesma origem de imagem da sua consulta de "produtos mais recentes".
                NmUrlImagem = p.NmUrlImagem
            })
            .ToListAsync();

        var temMais = itens.Count > TamanhoPagina;
        if (temMais)
            itens.RemoveAt(TamanhoPagina);

        return new ResultadoFeed(itens, total, temMais);
    }

    public async Task<List<DTOCategoriaFiltro>> ListarCategoriasAsync()
    {
        return await _context.Produtos
            .AsNoTracking()
            .Where(p =>
                        p.IcProdutoAtivo == true &&
                        p.CdLojaVirtualNavigation.IcLojaAtiva == true)
            .GroupBy(p => new { p.CdCategoria, p.CdCategoriaNavigation.NmCategoria })
            .Select(g => new DTOCategoriaFiltro
            {
                CdCategoria = g.Key.CdCategoria,
                NmCategoria = g.Key.NmCategoria,
                QtProdutos = g.Count()
            })
            .OrderBy(c => c.NmCategoria)
            .ToListAsync();
    }

    private IQueryable<Produto> AplicarFiltros(FiltroProdutos f)
    {
        var query = _context.Produtos
            .AsNoTracking()
            .Where(p =>
                    p.IcProdutoAtivo == true &&
                    p.CdLojaVirtualNavigation.IcLojaAtiva == true);

        if (f.Categorias.Count > 0)
            query = query.Where(p => f.Categorias.Contains(p.CdCategoria));

        if (f.PrecoMin is > 0)
            query = query.Where(p => p.VlProduto >= f.PrecoMin.Value);

        if (f.PrecoMax is > 0)
            query = query.Where(p => p.VlProduto <= f.PrecoMax.Value);

        if (f.ApenasEstoque)
            query = query.Where(p => p.QtdEstoque > 0);

        return query;
    }

    // Sempre com desempate por CdProduto: sem isso, produtos com a mesma data/preço
    // podem repetir ou sumir entre uma página e outra.
    private IOrderedQueryable<Produto> Ordenar(IQueryable<Produto> query, string? ordem) => ordem switch
    {
        "antigos" => query.OrderBy(p => p.DtCriacao).ThenBy(p => p.CdProduto),

        "menor-preco" => query.OrderBy(p => p.VlProduto).ThenBy(p => p.CdProduto),

        "maior-preco" => query.OrderByDescending(p => p.VlProduto).ThenBy(p => p.CdProduto),

        "nome" => query.OrderBy(p => p.NmProduto).ThenBy(p => p.CdProduto),

        "mais-vendidos" => query
            .OrderByDescending(p => _context.ItemPedidos
                .Where(i => i.CdProduto == p.CdProduto && i.CdPedidoNavigation.SgStatusPedido != "C")
                .Sum(i => (int?)i.QtItem) ?? 0)
            .ThenBy(p => p.CdProduto),

        _ => query.OrderByDescending(p => p.DtCriacao).ThenByDescending(p => p.CdProduto)
    };
}