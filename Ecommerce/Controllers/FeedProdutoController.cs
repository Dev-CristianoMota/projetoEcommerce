using Ecommerce.Services;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class FeedProdutoController : Controller
    {
        private readonly ProdutoFeedService _feedService;

        public FeedProdutoController(ProdutoFeedService feedService)
        {
            _feedService = feedService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] FiltroProdutos filtro,
            int pagina = 1)
        {
            var resultado = await _feedService.BuscarAsync(filtro, pagina);

            // Chamada do scroll infinito / filtros (fetch): devolve só os cards + metadados nos headers
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (resultado.Total is not null)
                    Response.Headers["X-Total"] = resultado.Total.Value.ToString();

                Response.Headers["X-Tem-Mais"] = resultado.TemMais ? "true" : "false";
                Response.Headers["Cache-Control"] = "no-store";

                return PartialView("_ProdutoCards", resultado.Itens);
            }

            // Carga normal da página (primeiros 50 renderizados no servidor)
            var vm = new FeedProdutosViewModel
            {
                Filtro = filtro,
                Produtos = resultado.Itens,
                Total = resultado.Total ?? 0,
                TemMais = resultado.TemMais,
                Categorias = await _feedService.ListarCategoriasAsync(),
                Lojas = await _feedService.ListarLojasAsync()
            };

            return View(vm);
        }
    }
}