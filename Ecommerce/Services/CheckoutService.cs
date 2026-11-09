using Ecommerce.Data;
using Ecommerce.DTO;
using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services
{
    public class CheckoutService
    {
        private readonly EcommerceDbContext _context;
        private readonly CarrinhoService _carrinhoService;
        private readonly CupomService _cupomService;

        public CheckoutService(EcommerceDbContext context, CarrinhoService carrinhoService, CupomService cupomService)
        {
            _context = context;
            _carrinhoService = carrinhoService;
            _cupomService = cupomService;
        }

        // ===== Tela de checkout =====
        public async Task<CheckoutViewModel> MontarAsync(ISession session, int cdUsuario, int? cdEnderecoSelecionado = null)
        {
            var carrinho = await _carrinhoService.ObterAsync(session, cdUsuario);
            await _cupomService.AplicarNoCarrinhoAsync(session, cdUsuario, carrinho);

            var enderecos = await _context.Enderecos
                .Where(e => e.CdUsuario == cdUsuario && e.IcEnderecoAtivo == true)
                .OrderByDescending(e => e.IcEnderecoPrincipal)
                .ThenBy(e => e.NmIdentificacao)
                .Select(e => new CheckoutEnderecoViewModel
                {
                    CdEndereco = e.CdEndereco,
                    NmIdentificacao = e.NmIdentificacao,
                    NmDestinatario = e.NmDestinatario,
                    NmCep = e.CEP,
                    NmLogradouro = e.Logradouro,
                    NmNumero = e.Numero,
                    DsComplemento = e.DsComplemento,
                    NmBairro = e.Bairro,
                    NmCidade = e.Cidade,
                    SgUf = e.UF,
                    IcPrincipal = e.IcEnderecoPrincipal == true
                })
                .ToListAsync();

            return new CheckoutViewModel
            {
                Carrinho = carrinho,
                Lojas = AgruparPorLoja(carrinho),
                Enderecos = enderecos,
                CdEnderecoSelecionado = cdEnderecoSelecionado ?? enderecos.FirstOrDefault()?.CdEndereco
            };
        }

        // ===== Confirmação do pedido =====
        public async Task<DTOResultadoCheckout> FinalizarAsync(ISession session, int cdUsuario, int cdEndereco)
        {
            var enderecoValido = await _context.Enderecos.AnyAsync(e =>
                e.CdEndereco == cdEndereco && e.CdUsuario == cdUsuario && e.IcEnderecoAtivo == true);

            if (!enderecoValido) return Falha("Selecione um endereço de entrega válido.");

            // Compara o que estava na Session com o que o banco permite hoje
            var bruto = _carrinhoService.LerItens(session, cdUsuario);
            var carrinho = await _carrinhoService.ObterAsync(session, cdUsuario);

            if (carrinho.Itens.Count == 0) return Falha("Seu carrinho está vazio.");

            var mudou = bruto.Count != carrinho.Itens.Count
                || bruto.Any(b => carrinho.Itens.FirstOrDefault(i => i.CdProduto == b.CdProduto)?.QtItem != b.QtItem);

            if (mudou)
                return Falha("Alguns itens mudaram de estoque ou disponibilidade e foram ajustados. Revise e confirme novamente.");

            // Revalida o cupom (validade e uso único)
            await _cupomService.AplicarNoCarrinhoAsync(session, cdUsuario, carrinho);
            if (!string.IsNullOrEmpty(carrinho.ErroCupom))
                return Falha($"{carrinho.ErroCupom} O cupom foi removido; revise o total e confirme novamente.");

            // Se algo falhar, o Dispose da transação desfaz tudo
            await using var transacao = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1) Baixa o estoque, sem deixar ficar negativo
                foreach (var item in carrinho.Itens)
                {
                    var qtd = item.QtItem;
                    var linhas = await _context.Produtos
                        .Where(p => p.CdProduto == item.CdProduto && p.QtdEstoque >= qtd)
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.QtdEstoque, p => p.QtdEstoque - qtd));

                    if (linhas == 0)
                        return Falha($"O produto \"{item.NmProduto}\" não tem mais estoque suficiente.");
                }

                // 2) UM pedido com todos os itens, com o preço congelado
                var pedido = new Pedido
                {
                    CdUsuario = cdUsuario,
                    CdCupom = carrinho.CdCupom,
                    CdEnderecoEntrega = cdEndereco,
                    SgStatusPedido = "P",
                    VlSubtotalPedido = carrinho.Subtotal,
                    VlDescontoPedido = carrinho.Desconto,
                    VlTotalPedido = carrinho.Total,
                    DtCriacaoPedido = DateTime.Now
                };

                foreach (var item in carrinho.Itens)
                {
                    pedido.ItemPedidos.Add(new ItemPedido
                    {
                        CdProduto = item.CdProduto,
                        QtItem = item.QtItem,
                        VlPrecoUnitario = item.VlProduto
                    });
                }

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
                await transacao.CommitAsync();

                // 3) Só limpa carrinho e cupom depois de gravar
                _carrinhoService.Limpar(session, cdUsuario);
                _cupomService.Remover(session, cdUsuario);

                return new DTOResultadoCheckout { Sucesso = true, CdPedido = pedido.CdPedido };
            }
            catch
            {
                _context.ChangeTracker.Clear();
                return Falha("Não foi possível finalizar a compra. Tente novamente.");
            }
        }

        public async Task<ConfirmacaoPedidoViewModel?> ObterConfirmacaoAsync(int cdUsuario, int cdPedido)
        {
            var dados = await _context.Pedidos
                .AsNoTracking()
                .Where(p => p.CdPedido == cdPedido && p.CdUsuario == cdUsuario)
                .Select(p => new
                {
                    p.CdPedido,
                    p.DtCriacaoPedido,
                    p.VlSubtotalPedido,
                    p.VlDescontoPedido,
                    p.VlTotalPedido,
                    CodigoCupom = p.CdCupomNavigation != null ? p.CdCupomNavigation.CdPromocional : null,
                    Endereco = p.CdEnderecoEntregaNavigation == null ? null : new CheckoutEnderecoViewModel
                    {
                        CdEndereco = p.CdEnderecoEntregaNavigation.CdEndereco,
                        NmIdentificacao = p.CdEnderecoEntregaNavigation.NmIdentificacao,
                        NmDestinatario = p.CdEnderecoEntregaNavigation.NmDestinatario,
                        NmCep = p.CdEnderecoEntregaNavigation.CEP,
                        NmLogradouro = p.CdEnderecoEntregaNavigation.Logradouro,
                        NmNumero = p.CdEnderecoEntregaNavigation.Numero,
                        DsComplemento = p.CdEnderecoEntregaNavigation.DsComplemento,
                        NmBairro = p.CdEnderecoEntregaNavigation.Bairro,
                        NmCidade = p.CdEnderecoEntregaNavigation.Cidade,
                        SgUf = p.CdEnderecoEntregaNavigation.UF,
                        IcPrincipal = p.CdEnderecoEntregaNavigation.IcEnderecoPrincipal == true
                    },
                    Itens = p.ItemPedidos.Select(i => new
                    {
                        i.CdProduto,
                        NmProduto = i.CdProdutoNavigation.NmProduto,
                        i.QtItem,
                        i.VlPrecoUnitario,
                        CdLojaVirtual = i.CdProdutoNavigation.CdLojaVirtual,
                        NmLojaVirtual = i.CdProdutoNavigation.CdLojaVirtualNavigation.NmLojaVirtual
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (dados == null) return null;

            var lojas = dados.Itens
                .GroupBy(i => new { i.CdLojaVirtual, i.NmLojaVirtual })
                .OrderBy(g => g.Key.NmLojaVirtual)
                .Select(g => new CheckoutLojaViewModel
                {
                    CdLojaVirtual = g.Key.CdLojaVirtual,
                    NmLojaVirtual = g.Key.NmLojaVirtual,
                    Subtotal = g.Sum(i => i.VlPrecoUnitario * i.QtItem),
                    Itens = g.Select(i => new CarrinhoItemViewModel
                    {
                        CdProduto = i.CdProduto,
                        NmProduto = i.NmProduto,
                        QtItem = i.QtItem,
                        VlProduto = i.VlPrecoUnitario,
                        CdLojaVirtual = g.Key.CdLojaVirtual,
                        NmLojaVirtual = g.Key.NmLojaVirtual
                    }).ToList()
                })
                .ToList();

            return new ConfirmacaoPedidoViewModel
            {
                CdPedido = dados.CdPedido,
                DtCriacao = dados.DtCriacaoPedido,
                QtItens = dados.Itens.Sum(i => i.QtItem),
                VlSubtotal = dados.VlSubtotalPedido,
                VlDesconto = dados.VlDescontoPedido,
                VlTotal = dados.VlTotalPedido,
                CodigoCupom = dados.CodigoCupom,
                Endereco = dados.Endereco,
                Lojas = lojas
            };
        }

        private static List<CheckoutLojaViewModel> AgruparPorLoja(CarrinhoViewModel carrinho)
            => carrinho.Itens
                .GroupBy(i => new { i.CdLojaVirtual, i.NmLojaVirtual })
                .Select(g => new CheckoutLojaViewModel
                {
                    CdLojaVirtual = g.Key.CdLojaVirtual,
                    NmLojaVirtual = g.Key.NmLojaVirtual,
                    Itens = g.ToList(),
                    Subtotal = g.Sum(i => i.VlProduto * i.QtItem)
                })
                .ToList();

        private static DTOResultadoCheckout Falha(string erro)
            => new() { Sucesso = false, Erro = erro };
    }
}