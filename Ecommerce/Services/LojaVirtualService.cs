using Ecommerce.Data;
using Ecommerce.DTO;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services
{
    public class LojaVirtualService
    {
        private readonly EcommerceDbContext _context;

        public LojaVirtualService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<DTOLojasMaisCompradas>> ObterLojasMaisCompradasAsync(int quantidade = 3)
        {
            var lojas = await _context.ItemPedidos
                .Where(i => i.CdPedidoNavigation.SgStatusPedido != "C"
                         && i.CdProdutoNavigation.CdLojaVirtualNavigation.IcLojaAtiva == true)
                .GroupBy(i => new
                {
                    i.CdProdutoNavigation.CdLojaVirtual,
                    i.CdProdutoNavigation.CdLojaVirtualNavigation.NmLojaVirtual,
                    i.CdProdutoNavigation.CdLojaVirtualNavigation.DsLojaVirtual
                })
                .OrderByDescending(g => g.Select(i => i.CdPedido).Distinct().Count())
                .ThenByDescending(g => g.Sum(i => i.QtItem * i.VlPrecoUnitario))
                .Take(quantidade)
                .Select(g => new DTOLojasMaisCompradas
                {
                    CdLojaVirtual = g.Key.CdLojaVirtual,
                    NmLojaVirtual = g.Key.NmLojaVirtual,
                    DsLojaVirtual = g.Key.DsLojaVirtual,
                    QtPedidos = g.Select(i => i.CdPedido).Distinct().Count()
                })
                .ToListAsync();

            if (lojas.Count < quantidade)
            {
                var idsJaListados = lojas.Select(l => l.CdLojaVirtual).ToList();

                var complemento = await _context.LojaVirtuals
                    .Where(l => l.IcLojaAtiva == true
                             && l.Produtos.Any(p => p.IcProdutoAtivo == true)
                             && !idsJaListados.Contains(l.CdLojaVirtual))
                    .OrderBy(l => l.CdLojaVirtual)
                    .Take(quantidade - lojas.Count)
                    .Select(l => new DTOLojasMaisCompradas
                    {
                        CdLojaVirtual = l.CdLojaVirtual,
                        NmLojaVirtual = l.NmLojaVirtual,
                        DsLojaVirtual = l.DsLojaVirtual,
                        QtPedidos = 0
                    })
                    .ToListAsync();

                lojas.AddRange(complemento);
            }

            return lojas;
        }
    }
}