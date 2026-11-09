using Ecommerce.Data;
using Ecommerce.DTO;
using Ecommerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Ecommerce.Services
{
    public class CarrinhoService
    {
        private readonly EcommerceDbContext _context;

        public CarrinhoService(EcommerceDbContext context) => _context = context;

        private static string Chave(int cdUsuario) => $"Carrinho_{cdUsuario}";

        private static List<DTOItemCarrinhoSessao> Ler(ISession session, int cdUsuario)
        {
            var json = session.GetString(Chave(cdUsuario));
            return string.IsNullOrEmpty(json)
                ? new List<DTOItemCarrinhoSessao>()
                : JsonSerializer.Deserialize<List<DTOItemCarrinhoSessao>>(json) ?? new();
        }

        private static void Salvar(ISession session, int cdUsuario, List<DTOItemCarrinhoSessao> itens)
            => session.SetString(Chave(cdUsuario), JsonSerializer.Serialize(itens));

        // Usados pelo checkout
        public List<DTOItemCarrinhoSessao> LerItens(ISession session, int cdUsuario)
            => Ler(session, cdUsuario);

        public void Limpar(ISession session, int cdUsuario)
            => session.Remove(Chave(cdUsuario));

        public async Task<CarrinhoViewModel> ObterAsync(ISession session, int cdUsuario)
        {
            var itensSessao = Ler(session, cdUsuario);
            var ids = itensSessao.Select(i => i.CdProduto).ToList();

            var produtos = await _context.Produtos
                .Where(p => ids.Contains(p.CdProduto) && p.IcProdutoAtivo == true)
                .Select(p => new
                {
                    p.CdProduto,
                    p.CdLojaVirtual,
                    p.NmProduto,
                    p.VlProduto,
                    p.QtdEstoque,
                    p.NmUrlImagem,
                    NmLojaVirtual = p.CdLojaVirtualNavigation.NmLojaVirtual
                })
                .ToListAsync();

            var itens = new List<CarrinhoItemViewModel>();
            foreach (var itemSessao in itensSessao)
            {
                var p = produtos.FirstOrDefault(x => x.CdProduto == itemSessao.CdProduto);
                if (p == null || p.QtdEstoque <= 0) continue;

                itens.Add(new CarrinhoItemViewModel
                {
                    CdProduto = p.CdProduto,
                    CdLojaVirtual = p.CdLojaVirtual,
                    NmProduto = p.NmProduto,
                    NmLojaVirtual = p.NmLojaVirtual,
                    NmUrlImagem = p.NmUrlImagem,
                    VlProduto = p.VlProduto,
                    QtItem = Math.Min(itemSessao.QtItem, p.QtdEstoque)
                });
            }

            // Mantém a Session coerente com o que o banco permite hoje
            Salvar(session, cdUsuario, itens
                .Select(i => new DTOItemCarrinhoSessao { CdProduto = i.CdProduto, QtItem = i.QtItem })
                .ToList());

            return new CarrinhoViewModel { UsuarioLogado = true, Itens = itens };
        }

        public async Task<string?> AdicionarAsync(ISession session, int cdUsuario, int cdProduto)
        {
            var produto = await _context.Produtos
                .Where(p => p.CdProduto == cdProduto && p.IcProdutoAtivo == true)
                .Select(p => new { p.QtdEstoque })
                .FirstOrDefaultAsync();

            if (produto == null) return "Produto indisponível.";
            if (produto.QtdEstoque <= 0) return "Produto sem estoque.";

            var itens = Ler(session, cdUsuario);
            var item = itens.FirstOrDefault(i => i.CdProduto == cdProduto);

            if (item == null)
                itens.Insert(0, new DTOItemCarrinhoSessao { CdProduto = cdProduto, QtItem = 1 });
            else if (item.QtItem >= produto.QtdEstoque)
                return "Quantidade máxima em estoque atingida.";
            else
                item.QtItem++;

            Salvar(session, cdUsuario, itens);
            return null;
        }

        public async Task<string?> AlterarQuantidadeAsync(ISession session, int cdUsuario, int cdProduto, int quantidade)
        {
            var itens = Ler(session, cdUsuario);
            var item = itens.FirstOrDefault(i => i.CdProduto == cdProduto);
            if (item == null) return null;

            if (quantidade <= 0)
            {
                itens.Remove(item);
            }
            else
            {
                var estoque = await _context.Produtos
                    .Where(p => p.CdProduto == cdProduto)
                    .Select(p => p.QtdEstoque)
                    .FirstOrDefaultAsync();

                if (quantidade > estoque) return "Quantidade máxima em estoque atingida.";
                item.QtItem = quantidade;
            }

            Salvar(session, cdUsuario, itens);
            return null;
        }

        public void Remover(ISession session, int cdUsuario, int cdProduto)
        {
            var itens = Ler(session, cdUsuario);
            itens.RemoveAll(i => i.CdProduto == cdProduto);
            Salvar(session, cdUsuario, itens);
        }
    }
}