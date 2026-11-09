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
        private readonly LojaVirtualService _lojaService;

        public HomeController(EcommerceDbContext context, ProdutoService produtoService, CategoriaService categoriaService, LojaVirtualService lojaService)
        {
            _context = context;
            _produtoService = produtoService;
            _categoriaService = categoriaService;
            _lojaService = lojaService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Conexao = _context.Database.CanConnect();

            var categorias = await _categoriaService.ObterCategoriasMaisVendidasAsync(12);
            
            ViewBag.CategoriasMaisVendidas = categorias;

            ViewBag.ProdutosMaisVendidos = await _produtoService.ObterProdutosMaisVendidosPaginadosAsync();

            ViewBag.ProdutosMaisRecentes = await _produtoService.ObterProdutosMaisRecentesPaginadosAsync();

            ViewBag.LojasMaisCompradas = await _lojaService.ObterLojasMaisCompradasAsync(3);

            return View();
        }
    }
}