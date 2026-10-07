using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }
    }
}
