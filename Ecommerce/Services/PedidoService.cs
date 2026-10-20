using Ecommerce.Data;
using Ecommerce.DTO;
using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Ecommerce.Services
{
    public class PedidoService
    {
        private readonly EcommerceDbContext _context;

        public PedidoService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<int> BuscarUltimoPedidoAsync(int cdUsuario = 4)
        {
            int existe = await _context.Pedidos.CountAsync(p => p.CdUsuario == cdUsuario);
            Console.WriteLine($"cdUsuario={cdUsuario}, pedidos encontrados={existe}");

            return existe;

            //    Equivalente ao subselect: último pedido do usuário
            //   var cdPedido = await _context.Pedidos
            //       .AsNoTracking()
            //       .Where(p => p.CdUsuario == cdUsuario)
            //       .OrderByDescending(p => p.DtCriacaoPedido)
            //       .ThenByDescending(p => p.CdPedido)
            //       .Select(p => (int?)p.CdPedido)
            //       .FirstOrDefaultAsync();

            //    if (cdPedido is null)
            //        return null;

            //    Equivalente ao SELECT com os JOINs
            //    var linhas = await
            //    (
            //        from p in _context.Pedidos.AsNoTracking()
            //        join lv in _context.LojaVirtuals
            //            on p.CdLojaVirtual equals lv.CdLojaVirtual
            //        join c in _context.Cupoms
            //            on p.CdCupom equals c.CdCupom into cupons
            //        from c in cupons.DefaultIfEmpty()
            //        join ip in _context.ItemPedidos
            //            on p.CdPedido equals ip.CdPedido into itens
            //        from ip in itens.DefaultIfEmpty()
            //        join pr in _context.Produtos
            //            on ip.CdProduto equals pr.CdProduto into produtos
            //        from pr in produtos.DefaultIfEmpty()
            //        where p.CdPedido == cdPedido
            //        orderby ip.CdProduto
            //        select new UltimoPedidoDTO
            //        {
            //            CdPedido = p.CdPedido,
            //            CdUsuario = p.CdUsuario,
            //            DtCriacaoPedido = p.DtCriacaoPedido,
            //            SgStatusPedido = p.SgStatusPedido,
            //            CdLojaVirtual = lv.CdLojaVirtual,
            //            NmLojaVirtual = lv.NmLojaVirtual,
            //            VlSubtotalPedido = p.VlSubtotalPedido,
            //            VlDescontoPedido = p.VlDescontoPedido,
            //            VlTotalPedido = p.VlTotalPedido,
            //            CdPromocional = c.CdPromocional,
            //            CdProduto = (int?)ip.CdProduto,
            //            NmProduto = pr.NmProduto,
            //            QtItem = (int?)ip.QtItem,
            //            VlPrecoUnitario = (decimal?)ip.VlPrecoUnitario,
            //            VlTotalItem = (decimal?)(ip.QtItem * ip.VlPrecoUnitario)
            //        }
            //    )
            //    .ToListAsync();

            //    if (linhas.Count == 0)
            //        return null;

            //    var cab = linhas[0];

            //    return new UltimoPedidoContaViewModel
            //    {
            //        CdPedido = cab.CdPedido,
            //        DtCriacao = cab.DtCriacaoPedido,
            //        SgStatus = cab.SgStatusPedido,
            //        NmLoja = cab.NmLojaVirtual,
            //        CdPromocional = cab.CdPromocional,
            //        VlSubtotal = cab.VlSubtotalPedido,
            //        VlDesconto = cab.VlDescontoPedido,
            //        VlTotal = cab.VlTotalPedido,
            //        Itens = linhas
            //            .Where(l => l.CdProduto.HasValue)
            //            .Select(l => new ItemUltimoPedidoViewModel
            //            {
            //                NmProduto = l.NmProduto ?? string.Empty,
            //                Quantidade = l.QtItem ?? 0,
            //                VlUnitario = l.VlPrecoUnitario ?? 0m
            //            })
            //            .ToList()
            //    };
        }
    }
}
