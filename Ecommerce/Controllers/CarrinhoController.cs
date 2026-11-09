using System.Security.Claims;
using Ecommerce.Services;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class CarrinhoController : BaseController
    {
        private readonly CarrinhoService _carrinhoService;
        private readonly CupomService _cupomService;

        public CarrinhoController(CarrinhoService carrinhoService, CupomService cupomService)
        {
            _carrinhoService = carrinhoService;
            _cupomService = cupomService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cdUsuario = ObterCdUsuarioLogado();
            if (cdUsuario == null)
                return RedirectToAction("Login", "Usuario",
                    new { returnUrl = Url.Action("Index", "Carrinho") });

            var vm = await _carrinhoService.ObterAsync(HttpContext.Session, cdUsuario.Value);
            await _cupomService.AplicarNoCarrinhoAsync(HttpContext.Session, cdUsuario.Value, vm);
            return View(vm);
        }

        [HttpGet]
        public Task<IActionResult> Sidebar(string? returnUrl)
            => RenderizarAsync(returnUrl);

        [HttpPost]
        public async Task<IActionResult> Adicionar(int cdProduto, string? returnUrl, string? origem)
        {
            string? aviso = null;
            var cdUsuario = ObterCdUsuarioLogado();
            if (cdUsuario != null)
                aviso = await _carrinhoService.AdicionarAsync(HttpContext.Session, cdUsuario.Value, cdProduto);

            return await RenderizarAsync(returnUrl, aviso, origem);
        }

        [HttpPost]
        public async Task<IActionResult> AlterarQuantidade(int cdProduto, int quantidade, string? returnUrl, string? origem)
        {
            string? aviso = null;
            var cdUsuario = ObterCdUsuarioLogado();
            if (cdUsuario != null)
                aviso = await _carrinhoService.AlterarQuantidadeAsync(HttpContext.Session, cdUsuario.Value, cdProduto, quantidade);

            return await RenderizarAsync(returnUrl, aviso, origem);
        }

        [HttpPost]
        public async Task<IActionResult> Remover(int cdProduto, string? returnUrl, string? origem)
        {
            var cdUsuario = ObterCdUsuarioLogado();
            if (cdUsuario != null)
                _carrinhoService.Remover(HttpContext.Session, cdUsuario.Value, cdProduto);

            return await RenderizarAsync(returnUrl, origem: origem);
        }

        [HttpPost]
        public async Task<IActionResult> AplicarCupom(string? codigo)
        {
            string? erro = null;
            var cdUsuario = ObterCdUsuarioLogado();
            if (cdUsuario != null)
                erro = await _cupomService.AplicarAsync(HttpContext.Session, cdUsuario.Value, codigo);

            return await RenderizarAsync(null, origem: "pagina", erroCupom: erro);
        }

        [HttpPost]
        public async Task<IActionResult> RemoverCupom()
        {
            var cdUsuario = ObterCdUsuarioLogado();
            if (cdUsuario != null)
                _cupomService.Remover(HttpContext.Session, cdUsuario.Value);

            return await RenderizarAsync(null, origem: "pagina");
        }

        private async Task<IActionResult> RenderizarAsync(
            string? returnUrl, string? aviso = null, string? origem = null, string? erroCupom = null)
        {
            var cdUsuario = ObterCdUsuarioLogado();

            var vm = cdUsuario == null
                ? new CarrinhoViewModel { UsuarioLogado = false }
                : await _carrinhoService.ObterAsync(HttpContext.Session, cdUsuario.Value);

            vm.Mensagem = aviso;
            vm.UrlLogin = Url.Action("Login", "Usuario", new { returnUrl }) ?? "/Usuario/Login";

            if (origem == "pagina")
            {
                vm.ErroCupom = erroCupom;
                if (cdUsuario != null)
                    await _cupomService.AplicarNoCarrinhoAsync(HttpContext.Session, cdUsuario.Value, vm);

                return PartialView("_CarrinhoLista", vm);
            }

            return PartialView("_CarrinhoSidebar", vm);
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