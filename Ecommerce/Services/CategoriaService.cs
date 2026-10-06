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
            return await _context.ItemPedidos
                .Where(i => i.CdPedidoNavigation.SgStatusPedido != "C")
                .GroupBy(i => new {
                    i.CdProdutoNavigation.CdCategoria,
                    i.CdProdutoNavigation.CdCategoriaNavigation.NmCategoria
                })
                .Select(g => new DTOCategoriaMaisVendida
                {
                    CdCategoria = g.Key.CdCategoria,
                    NmCategoria = g.Key.NmCategoria,
                    QtdVendida = g.Sum(i => i.QtItem),
                    QtdProdutosVendidos = g.Select(i => i.CdProduto).Distinct().Count()
                })
                .OrderByDescending(c => c.QtdVendida)
                .Take(quantidade)
                .ToListAsync();
        }
    }
}
