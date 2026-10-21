using Ecommerce.Data;
using Ecommerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Ecommerce.Services
{
    public class PedidoService
    {
        private const int TamanhoPagina = 5;

        private readonly EcommerceDbContext _context;

        public PedidoService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<MeusPedidosViewModel> ListarPedidosUsuarioAsync(int cdUsuario, string? status, string? ordem, int pagina)
        {
            if (pagina < 1) pagina = 1;

            var query = _context.Pedidos
                .AsNoTracking()
                .Where(p => p.CdUsuario == cdUsuario);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(p => p.SgStatusPedido == status);

            var totalPedidos = await query.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalPedidos / (double)TamanhoPagina);

            var maisAntigos = ordem == "antigos";

            var ordenada = maisAntigos
                ? query.OrderBy(p => p.DtCriacaoPedido).ThenBy(p => p.CdPedido)
                : query.OrderByDescending(p => p.DtCriacaoPedido).ThenByDescending(p => p.CdPedido);

            var pedidos = await ordenada
                .Skip((pagina - 1) * TamanhoPagina)
                .Take(TamanhoPagina)
                .Select(p => new ResumoPedidoViewModel
                {
                    CdPedido = p.CdPedido,
                    DtCriacao = p.DtCriacaoPedido,
                    NmLoja = p.CdLojaVirtualNavigation.NmLojaVirtual,
                    SgStatus = p.SgStatusPedido,
                    VlTotal = p.VlTotalPedido,
                    QtItens = p.ItemPedidos.Count()
                })
                .ToListAsync();

            return new MeusPedidosViewModel
            {
                Pedidos = pedidos,
                StatusFiltro = status,
                Ordem = maisAntigos ? "antigos" : "recentes",
                PaginaAtual = pagina,
                TotalPaginas = totalPaginas
            };
        }


        public async Task<DetalhePedidoViewModel?> ObterDetalhesPedidoAsync(int cdPedido, int cdUsuario)
        {
            return await _context.Pedidos
                .AsNoTracking()
                .Where(p => p.CdPedido == cdPedido && p.CdUsuario == cdUsuario)
                .Select(p => new DetalhePedidoViewModel
                {
                    CdPedido = p.CdPedido,
                    DtCriacao = p.DtCriacaoPedido,
                    NmLoja = p.CdLojaVirtualNavigation.NmLojaVirtual,
                    SgStatus = p.SgStatusPedido,
                    VlTotal = p.VlTotalPedido,
                    QtItens = p.ItemPedidos.Count(),

                    VlSubtotal = p.VlSubtotalPedido,
                    VlDesconto = p.VlDescontoPedido,
                    DtAtualizacao = p.DtAtualizacaoPedido,

                    CodigoCupom = p.CdCupomNavigation != null ? p.CdCupomNavigation.CdPromocional : null,
                    TipoDesconto = p.CdCupomNavigation != null ? p.CdCupomNavigation.SgTipoDesconto : null,
                    VlCupom = p.CdCupomNavigation != null ? p.CdCupomNavigation.VlDesconto : (decimal?)null,

                    Endereco = p.CdEnderecoEntregaNavigation == null ? null : new EnderecoEntregaViewModel
                    {
                        Cep = p.CdEnderecoEntregaNavigation.CEP,
                        Logradouro = p.CdEnderecoEntregaNavigation.Logradouro,
                        Numero = p.CdEnderecoEntregaNavigation.Numero,
                        Bairro = p.CdEnderecoEntregaNavigation.Bairro,
                        Cidade = p.CdEnderecoEntregaNavigation.Cidade,
                        Uf = p.CdEnderecoEntregaNavigation.UF
                    },

                    Itens = p.ItemPedidos
                        .Select(i => new ItemPedidoViewModel
                        {
                            NmProduto = i.CdProdutoNavigation.NmProduto,
                            NmCategoria = i.CdProdutoNavigation.CdCategoriaNavigation.NmCategoria,
                            Quantidade = i.QtItem,
                            VlUnitario = i.VlPrecoUnitario
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }


        public async Task<UltimoPedidoContaViewModel?> BuscarUltimoPedidoAsync(int cdUsuario)
        {
            return await _context.Pedidos
                .AsNoTracking()
                .Where(p => p.CdUsuario == cdUsuario)
                .OrderByDescending(p => p.DtCriacaoPedido)
                .ThenByDescending(p => p.CdPedido)
                .Select(p => new UltimoPedidoContaViewModel
                {
                    CdPedido = p.CdPedido,
                    DtCriacao = p.DtCriacaoPedido,
                    SgStatus = p.SgStatusPedido,
                    NmLoja = p.CdLojaVirtualNavigation.NmLojaVirtual,
                    CdPromocional = p.CdCupomNavigation != null ? p.CdCupomNavigation.CdPromocional : null,
                    VlSubtotal = p.VlSubtotalPedido,
                    VlDesconto = p.VlDescontoPedido,
                    VlTotal = p.VlTotalPedido,
                    Itens = p.ItemPedidos
                        .OrderBy(i => i.CdProduto)
                        .Select(i => new ItemUltimoPedidoViewModel
                        {
                            NmProduto = i.CdProdutoNavigation.NmProduto,
                            Quantidade = i.QtItem,
                            VlUnitario = i.VlPrecoUnitario
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
