using Ecommerce.Data;
using Ecommerce.DTO;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services;

public class ProdutoService
{
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
        
        if(produto == null)
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

    public async Task<List<DTOProdutoMaisVendido>> ObterMaisVendidosAsync(int quantidade = 5)
    {
        return await _context.ItemPedidos
            .Where(i => i.CdPedidoNavigation.SgStatusPedido != "C")
            .GroupBy(i => new { i.CdProduto, i.CdProdutoNavigation.NmProduto, i.CdProdutoNavigation.VlProduto })
            .Select(g => new DTOProdutoMaisVendido

            {
                CdProduto = g.Key.CdProduto,
                NmProduto = g.Key.NmProduto,
                VlProduto = g.Key.VlProduto,
                QtdVendida = g.Sum(i => i.QtItem)
            })
            .OrderByDescending(p => p.QtdVendida)
            .Take(quantidade)
            .ToListAsync();
    }
}