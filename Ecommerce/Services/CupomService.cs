using System.Globalization;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services
{
    public class CupomService
    {
        private readonly EcommerceDbContext _context;
        private static readonly CultureInfo Pt = new("pt-BR");

        public CupomService(EcommerceDbContext context) => _context = context;

        private static string Chave(int cdUsuario) => $"Cupom_{cdUsuario}";

        public async Task<string?> AplicarAsync(ISession session, int cdUsuario, string? codigo)
        {
            codigo = codigo?.Trim();
            if (string.IsNullOrEmpty(codigo)) return "Informe o código do cupom.";

            var cupom = await _context.Cupoms.FirstOrDefaultAsync(c => c.CdPromocional == codigo);

            var erro = await ValidarAsync(cdUsuario, cupom);
            if (erro != null) return erro;

            session.SetInt32(Chave(cdUsuario), cupom!.CdCupom);
            return null;
        }

        public void Remover(ISession session, int cdUsuario)
            => session.Remove(Chave(cdUsuario));

        public async Task AplicarNoCarrinhoAsync(ISession session, int cdUsuario, CarrinhoViewModel vm)
        {
            var cdCupom = session.GetInt32(Chave(cdUsuario));
            if (cdCupom == null) return;

            var cupom = await _context.Cupoms.FirstOrDefaultAsync(c => c.CdCupom == cdCupom);

            var erro = await ValidarAsync(cdUsuario, cupom);
            if (erro != null)
            {
                session.Remove(Chave(cdUsuario));
                vm.ErroCupom = erro;
                return;
            }

            vm.CodigoCupom = cupom!.CdPromocional;
            vm.Desconto = CalcularDesconto(cupom.SgTipoDesconto, cupom.VlDesconto, vm.Subtotal);
            vm.DescricaoCupom = cupom.SgTipoDesconto == "P"
                ? $"{cupom.VlDesconto.ToString("0.##", Pt)}% de desconto"
                : $"{cupom.VlDesconto.ToString("C", Pt)} de desconto";
        }

        public static decimal CalcularDesconto(string tipo, decimal valor, decimal subtotal)
        {
            var desconto = tipo == "P" ? Math.Round(subtotal * valor / 100m, 2) : valor;
            return Math.Min(desconto, subtotal);
        }

        public async Task<string?> ValidarAsync(int cdUsuario, Cupom? cupom)
        {
            if (cupom == null || cupom.IcCupomAtivo != true) return "Cupom inválido.";

            var agora = DateTime.Now;
            if (agora < cupom.DtInicioValidade) return "Este cupom ainda não está válido.";
            if (agora > cupom.DtFimValidade) return "Este cupom expirou.";

            var jaUsou = await _context.Pedidos.AnyAsync(p =>
                p.CdUsuario == cdUsuario &&
                p.CdCupom == cupom.CdCupom &&
                p.SgStatusPedido != "C");

            return jaUsou ? "Você já utilizou este cupom." : null;
        }
    }
}