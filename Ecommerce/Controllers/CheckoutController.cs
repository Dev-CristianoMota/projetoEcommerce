using Ecommerce.DTO;
using Ecommerce.Helpers;
using Ecommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class CheckoutController : BaseController
    {
        private readonly CheckoutService _checkoutService;

        public CheckoutController(CheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cdUsuario = ObterCdUsuarioLogado();
            if (cdUsuario == null) return RedirectToAction("Login", "Usuario");

            var vm = await _checkoutService.MontarAsync(HttpContext.Session, cdUsuario.Value);
            if (!vm.Lojas.Any()) return RedirectToAction("Index", "Carrinho");

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalizar(int cdEndereco)
        {
            var usuario = HttpContext.Session.GetObject<DTOUsuarioLogado>("UsuarioLogado")!;
            var resultado = await _checkoutService.FinalizarAsync(HttpContext.Session, usuario.CdUsuario, cdEndereco);

            if (!resultado.Sucesso)
            {
                TempData["Erro"] = resultado.Erro;
                return RedirectToAction("Index");
            }

            // O número do pedido fica na Session, e não na URL
            HttpContext.Session.SetInt32("PedidoConcluido", resultado.CdPedido);
            return RedirectToAction("Concluido");
        }

        [HttpGet]
        public async Task<IActionResult> Concluido()
        {
            var cdUsuario = ObterCdUsuarioLogado();
            if (cdUsuario == null) return RedirectToAction("Login", "Usuario");

            var cdPedido = HttpContext.Session.GetInt32("PedidoConcluido");
            if (cdPedido == null) return RedirectToAction("ListarPedidosUsuario", "Pedido");

            var vm = await _checkoutService.ObterConfirmacaoAsync(cdUsuario.Value, cdPedido.Value);
            if (vm == null) return RedirectToAction("ListarPedidosUsuario", "Pedido");

            return View(vm);
        }

        private int? ObterCdUsuarioLogado()
        {
            if (UsuarioLogado is not null)
                return UsuarioLogado.CdUsuario;

            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }
    }
}