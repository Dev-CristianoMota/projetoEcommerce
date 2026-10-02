using Ecommerce.Data;
using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly EcommerceDbContext _context;
        private readonly ProdutoService _produtoService;
        private readonly CategoriaService _categoriaService;

        public HomeController(EcommerceDbContext context, ProdutoService produtoService, CategoriaService categoriaService)
        {
            _context = context;
            _produtoService = produtoService;
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Conexao = _context.Database.CanConnect();

            var produtosMaisVendidos = await _produtoService.ObterMaisVendidosAsync(5);

            ViewBag.CategoriasMaisVendidas =
                await _categoriaService.ObterCategoriasMaisVendidasAsync(5);

            return View(produtosMaisVendidos);
        }
    }
}