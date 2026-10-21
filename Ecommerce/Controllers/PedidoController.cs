using Ecommerce.DTO;
using Ecommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Ecommerce.Controllers
{
    [Authorize]
    [Route("Usuario")]
    public class PedidoController : Controller
    {
        private static readonly string[] StatusValidos = { "P", "A", "E", "T", "C" };

        private static readonly string[] OrdensValidas = { "recentes", "antigos" };

        private readonly PedidoService _pedidoService;

        public PedidoController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        private int ObterUsuarioId() =>
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

        [HttpGet("")]
        public IActionResult Conta() => View("~/Views/Usuario/Conta.cshtml");

        [HttpGet("Pedidos")]
        public async Task<IActionResult> ListarPedidosUsuario(string? status, string? ordem, int pagina = 1)
        {
            var cdUsuario = ObterUsuarioId();
            if (cdUsuario <= 0)
                return RedirectToAction("Login", "Usuario");

            if (!string.IsNullOrWhiteSpace(status) && !StatusValidos.Contains(status))
                status = null;

            if (string.IsNullOrWhiteSpace(ordem) || !OrdensValidas.Contains(ordem))
                ordem = "recentes";

            var vm = await _pedidoService.ListarPedidosUsuarioAsync(cdUsuario, status, ordem, pagina);
            return View("~/Views/Usuario/MeusPedidos.cshtml", vm);
        }

        [HttpGet("Pedidos/{id}")]
        public async Task<IActionResult> Detalhes(int id)
        {
            var cdUsuario = ObterUsuarioId();
            if (cdUsuario <= 0)
                return RedirectToAction("Login", "Usuario");

            var vm = await _pedidoService.ObterDetalhesPedidoAsync(id, cdUsuario);

            if (vm is null)
                return NotFound();

            return View("~/Views/Usuario/MeusPedidosDetalhes.cshtml", vm);
        }

        //public async Task<IActionResult> Ultimo()
        //{
        //    var cdUsuario = ObterUsuarioId();
        //    if (cdUsuario <= 0)
        //        return RedirectToAction("Login", "Usuario");

        //    var vm = await _pedidoService.BuscarUltimoPedidoAsync(cdUsuario);

        //    return View(vm);
        //}
    }
}