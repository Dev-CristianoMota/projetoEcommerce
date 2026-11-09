using Ecommerce.Data;
using Ecommerce.ViewModels;
using Microsoft.EntityFrameworkCore;

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

            var dados = await ordenada
                .Skip((pagina - 1) * TamanhoPagina)
                .Take(TamanhoPagina)
                .Select(p => new
                {
                    p.CdPedido,
                    p.DtCriacaoPedido,
                    p.SgStatusPedido,
                    p.VlTotalPedido,
                    QtItens = p.ItemPedidos.Count(),
                    Lojas = p.ItemPedidos
                        .Select(i => i.CdProdutoNavigation.CdLojaVirtualNavigation.NmLojaVirtual)
                        .ToList()
                })
                .ToListAsync();

            var pedidos = dados.Select(d => new ResumoPedidoViewModel
            {
                CdPedido = d.CdPedido,
                DtCriacao = d.DtCriacaoPedido,
                NmLoja = RotuloLojas(d.Lojas),
                Lojas = d.Lojas.Distinct().OrderBy(l => l).ToList(),
                SgStatus = d.SgStatusPedido,
                VlTotal = d.VlTotalPedido,
                QtItens = d.QtItens
            }).ToList();

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
            var vm = await _context.Pedidos
                .AsNoTracking()
                .Where(p => p.CdPedido == cdPedido && p.CdUsuario == cdUsuario)
                .Select(p => new DetalhePedidoViewModel
                {
                    CdPedido = p.CdPedido,
                    DtCriacao = p.DtCriacaoPedido,
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

                    // Ordenado por loja para o agrupamento ficar natural na tela
                    Itens = p.ItemPedidos
                        .OrderBy(i => i.CdProdutoNavigation.CdLojaVirtualNavigation.NmLojaVirtual)
                        .ThenBy(i => i.CdProdutoNavigation.NmProduto)
                        .Select(i => new ItemPedidoViewModel
                        {
                            cdProduto = i.CdProduto,
                            cdCategoria = i.CdProdutoNavigation.CdCategoriaNavigation.CdCategoria,
                            NmLoja = i.CdProdutoNavigation.CdLojaVirtualNavigation.NmLojaVirtual,
                            NmProduto = i.CdProdutoNavigation.NmProduto,
                            NmCategoria = i.CdProdutoNavigation.CdCategoriaNavigation.NmCategoria,
                            NmUrlImagem = i.CdProdutoNavigation.NmUrlImagem,
                            Quantidade = i.QtItem,
                            VlUnitario = i.VlPrecoUnitario
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (vm is null) return null;

            vm.Lojas = vm.Itens.Select(i => i.NmLoja).Distinct().ToList();
            vm.NmLoja = RotuloLojas(vm.Lojas);

            return vm;
        }

        public async Task<UltimoPedidoContaViewModel?> BuscarUltimoPedidoAsync(int cdUsuario)
        {
            var dados = await _context.Pedidos
                .AsNoTracking()
                .Where(p => p.CdUsuario == cdUsuario)
                .OrderByDescending(p => p.DtCriacaoPedido)
                .ThenByDescending(p => p.CdPedido)
                .Select(p => new
                {
                    p.CdPedido,
                    p.DtCriacaoPedido,
                    p.SgStatusPedido,
                    p.VlSubtotalPedido,
                    p.VlDescontoPedido,
                    p.VlTotalPedido,
                    CdPromocional = p.CdCupomNavigation != null ? p.CdCupomNavigation.CdPromocional : null,
                    Itens = p.ItemPedidos
                        .Select(i => new ItemUltimoPedidoViewModel
                        {
                            NmProduto = i.CdProdutoNavigation.NmProduto,
                            Quantidade = i.QtItem,
                            VlUnitario = i.VlPrecoUnitario
                        })
                        .ToList(),
                    NmLojas = p.ItemPedidos
                        .Select(i => i.CdProdutoNavigation.CdLojaVirtualNavigation.NmLojaVirtual)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (dados is null)
                return null;

            return new UltimoPedidoContaViewModel
            {
                CdPedido = dados.CdPedido,
                NumeroFormatado = $"#{dados.CdPedido:D6}",
                DtCriacao = dados.DtCriacaoPedido,
                SgStatus = dados.SgStatusPedido,
                NmLoja = RotuloLojas(dados.NmLojas),
                CdPromocional = dados.CdPromocional,
                VlSubtotal = dados.VlSubtotalPedido,
                VlDesconto = dados.VlDescontoPedido,
                VlTotal = dados.VlTotalPedido,
                Itens = dados.Itens
            };
        }


        // "Loja A" quando é uma só; "Loja A +2 lojas" quando são várias
        private static string RotuloLojas(IEnumerable<string> lojas)
        {
            var lista = lojas.Distinct().OrderBy(l => l).ToList();

            return lista.Count switch
            {
                0 => "—",
                1 => lista[0],
                _ => $"{lista[0]} +{lista.Count - 1} {(lista.Count - 1 == 1 ? "loja" : "lojas")}"
            };
        }
    }
}