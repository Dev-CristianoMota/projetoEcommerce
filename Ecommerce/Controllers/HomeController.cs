using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly EcommerceDbContext _context;

        public HomeController(EcommerceDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var conexao = _context.Database.CanConnect();

            ViewBag.Conexao = conexao;

            return View();
        }
    }
}