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

        public HomeController(EcommerceDbContext context, ProdutoService produtoService, CategoriaService categoriaService, UsuarioService usuarioService)
        {
            _context = context;
            _produtoService = produtoService;
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Conexao = _context.Database.CanConnect();

            var categorias = await _categoriaService.ObterCategoriasMaisVendidasAsync(12);
            
            ViewBag.CategoriasMaisVendidas = categorias;

            return View();
        }
    }
}