using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

public class ProdutoController : Controller
{
    private readonly ProdutoService _produtoService;

    public ProdutoController(ProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    public async Task<IActionResult> Index()
    {
        var produtos = await _produtoService.ListarProdutosAsync();

        return View(produtos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var produto = await _produtoService.BuscarPorIdAsync(id);

        if (produto == null)
        {
            return NotFound();
        }

        return View(produto);
    }
}