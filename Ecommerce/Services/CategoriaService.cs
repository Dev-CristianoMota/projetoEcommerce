using Ecommerce.Data;
using Ecommerce.DTO;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services
{
    public class CategoriaService
    {
        private readonly EcommerceDbContext _context;

        public CategoriaService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<DTOCategoriaMaisVendida>> ObterCategoriasMaisVendidasAsync(int quantidade)
        {
            return await _context.Categoria
                .Select(c => new DTOCategoriaMaisVendida
                {
                    CdCategoria = c.CdCategoria,
                    NmCategoria = c.NmCategoria,
                    QtdVendida = c.Produtos
                        .SelectMany(p => p.ItemPedidos)
                        .Where(i => i.CdPedidoNavigation.SgStatusPedido != "C")
                        .Sum(i => (int?)i.QtItem) ?? 0,
                    QtdProdutosVendidos = c.Produtos
                        .Count(p => p.ItemPedidos.Any(i => i.CdPedidoNavigation.SgStatusPedido != "C"))
                })
                .OrderByDescending(c => c.QtdVendida)
                .ThenBy(c => c.NmCategoria)
                .Take(quantidade)
                .ToListAsync();
        }
    }
}
