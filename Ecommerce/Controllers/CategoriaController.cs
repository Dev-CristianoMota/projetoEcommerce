using Ecommerce.Services;

namespace Ecommerce.Controllers
{
    public class CategoriaController
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }


    }
}
