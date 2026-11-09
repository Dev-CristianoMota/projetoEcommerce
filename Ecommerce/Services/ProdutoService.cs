using Ecommerce.Data;
using Ecommerce.DTO;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services;

public class ProdutoService
{
    private const int TamanhoPagina = 5;
    private const int TotalMostrado = 15;

    private readonly EcommerceDbContext _context;

    public ProdutoService(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<List<Produto>> ListarProdutosAsync()
    {
        return await _context.Produtos
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Produto?> BuscarPorIdAsync(int id)
    {
        return await _context.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.CdProduto == id);
    }

    public async Task<DTOProdutoDetalhe?> BuscarDetalheAsync(int id)
    {
        return await _context.Produtos
            .AsNoTracking()
            .Where(p => p.CdProduto == id
                     && p.IcProdutoAtivo == true
                     && p.CdLojaVirtualNavigation.IcLojaAtiva == true)
            .Select(p => new DTOProdutoDetalhe
            {
                CdProduto = p.CdProduto,
                NmProduto = p.NmProduto,
                DsProduto = p.DsProduto,
                VlProduto = p.VlProduto,
                QtEstoque = p.QtdEstoque,
                NmUrlImagem = p.NmUrlImagem,
                DtCriacao = p.DtCriacao,
                CdLojaVirtual = p.CdLojaVirtual,
                NmLojaVirtual = p.CdLojaVirtualNavigation.NmLojaVirtual,
                CdCategoria = p.CdCategoria,
                NmCategoria = p.CdCategoriaNavigation.NmCategoria
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<DTOProdutoSugestao>> BuscarSugestoesAsync(string termo, int quantidade = 6)
    {
        var query = _context.Produtos
            .AsNoTracking()
            .Where(p => p.IcProdutoAtivo == true
                     && p.CdLojaVirtualNavigation.IcLojaAtiva == true);

        foreach (var palavra in termo.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var t = palavra;
            query = query.Where(p => EF.Functions.Like(p.NmProduto, $"%{t}%"));
        }

        return await query
            .OrderBy(p => p.NmProduto)
            .Take(quantidade)
            .Select(p => new DTOProdutoSugestao
            {
                CdProduto = p.CdProduto,
                NmProduto = p.NmProduto,
                VlProduto = p.VlProduto,
                NmUrlImagem = p.NmUrlImagem
            })
            .ToListAsync();
    }

    public async Task<Produto> CriarProdutoAsync(Produto produto)
    {
        produto.DtCriacao = DateTime.Now;
        produto.DtAtualizacao = DateTime.Now;
        produto.IcProdutoAtivo = true;

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task<Produto?> AtualizarProdutoAsync(int id, Produto produtoAtualizado)
    {
        var produtoExistente = await _context.Produtos.FindAsync(id);

        if (produtoExistente == null)
        {
            return null;
        }

        produtoExistente.CdCategoria = produtoAtualizado.CdCategoria;
        produtoExistente.NmProduto = produtoAtualizado.NmProduto;
        produtoExistente.DsProduto = produtoAtualizado.DsProduto;
        produtoExistente.VlProduto = produtoAtualizado.VlProduto;
        //produtoExistente.QtdEstoque = produtoAtualizado.QtdEstoque;
        produtoExistente.DtAtualizacao = DateTime.Now;
        await _context.SaveChangesAsync();

        return produtoExistente;
    }

    public async Task<bool> DesativarProdutoAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null)
        {
            return false;
        }

        produto.IcProdutoAtivo = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AtivarProdutoAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null)
        {
            return false;
        }

        produto.IcProdutoAtivo = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Produto>> ListarProdutosPorCategoriaAsync(int cdCategoria)
    {
        return await _context.Produtos
            .AsNoTracking()
            .Where(p => p.CdCategoria == cdCategoria)
            .ToListAsync();
    }

    public async Task<List<DTOProdutoMaisVendido>> ObterProdutosMaisVendidosAsync(int quantidade = TotalMostrado)
    {
        return await _context.ItemPedidos
            .AsNoTracking()
            .Where(i => i.CdPedidoNavigation.SgStatusPedido != "C"
                     && i.CdProdutoNavigation.IcProdutoAtivo == true
                     && i.CdProdutoNavigation.CdLojaVirtualNavigation.IcLojaAtiva == true)
            .GroupBy(i => new
            {
                i.CdProduto,
                i.CdProdutoNavigation.NmProduto,
                i.CdProdutoNavigation.VlProduto,
                i.CdProdutoNavigation.CdLojaVirtualNavigation.NmLojaVirtual,
                i.CdProdutoNavigation.NmUrlImagem
            })
            .Select(g => new DTOProdutoMaisVendido
            {
                CdProduto = g.Key.CdProduto,
                NmProduto = g.Key.NmProduto,
                VlProduto = g.Key.VlProduto,
                NmLojaVirtual = g.Key.NmLojaVirtual,
                NmUrlImagem = g.Key.NmUrlImagem,
                QtdVendida = g.Sum(i => i.QtItem)
            })
            .OrderByDescending(p => p.QtdVendida)
            .ThenBy(p => p.CdProduto)
            .Take(quantidade)
            .ToListAsync();
    }

    public async Task<List<List<DTOProdutoMaisVendido>>> ObterProdutosMaisVendidosPaginadosAsync()
    {
        var produtos = await ObterProdutosMaisVendidosAsync(TotalMostrado);

        return produtos
            .Chunk(TamanhoPagina)
            .Select(pagina => pagina.ToList())
            .ToList();
    }

    public async Task<List<DTOProdutoMaisRecentes>> ObterProdutosMaisRecentesAsync(int quantidade = TotalMostrado)
    {
        return await _context.Produtos
            .AsNoTracking()
            .Where(p => p.IcProdutoAtivo == true
                     && p.CdLojaVirtualNavigation.IcLojaAtiva == true)
            .OrderByDescending(p => p.DtCriacao)
            .ThenByDescending(p => p.CdProduto)
            .Take(quantidade)
            .Select(p => new DTOProdutoMaisRecentes
            {
                CdProduto = p.CdProduto,
                NmProduto = p.NmProduto,
                VlProduto = p.VlProduto,
                NmLojaVirtual = p.CdLojaVirtualNavigation.NmLojaVirtual,
                DtCriacao = p.DtCriacao,
                NmUrlImagem = p.NmUrlImagem
            })
            .ToListAsync();
    }

    public async Task<List<List<DTOProdutoMaisRecentes>>> ObterProdutosMaisRecentesPaginadosAsync()
    {
        var produtos = await ObterProdutosMaisRecentesAsync(TotalMostrado);

        return produtos
            .Chunk(TamanhoPagina)
            .Select(pagina => pagina.ToList())
            .ToList();
    }
}