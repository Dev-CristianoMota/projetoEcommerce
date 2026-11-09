using Ecommerce.Services;
using Ecommerce.DTO;
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

    public async Task<IActionResult> Detalhes(int id)
    {
        var produto = await _produtoService.BuscarDetalheAsync(id);

        if (produto == null)
            return NotFound();

        return View(produto);
    }

    [HttpGet]
    public async Task<IActionResult> Sugestoes(string? termo)
    {
        if (string.IsNullOrWhiteSpace(termo) || termo.Trim().Length < 2)
            return Json(new List<DTOProdutoSugestao>());

        return Json(await _produtoService.BuscarSugestoesAsync(termo));
    }
}