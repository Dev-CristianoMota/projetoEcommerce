using Ecommerce.Data;
using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ImportacaoController : Controller
{
    private readonly EcommerceDbContext _context;
    private readonly ImagemService _imagemService;
    private readonly IWebHostEnvironment _env;

    public ImportacaoController(EcommerceDbContext context, ImagemService imagemService, IWebHostEnvironment env)
    {
        _context = context;
        _imagemService = imagemService;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Imagens()
    {
        if (!_env.IsDevelopment()) return NotFound();

        ViewBag.Produtos = await _context.Produtos
            .OrderBy(p => p.CdProduto)
            .Select(p => new { p.CdProduto, p.NmProduto, p.NmUrlImagem })
            .ToListAsync();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Imagens(int cdProduto, IFormFile imagem)
    {
        if (!_env.IsDevelopment()) return NotFound();

        var produto = await _context.Produtos.FindAsync(cdProduto);
        if (produto == null) return NotFound();

        try
        {
            var (url, publicId) = await _imagemService.EnviarAsync(imagem);

            await _imagemService.ExcluirAsync(produto.CdPublicIdImagem);

            produto.NmUrlImagem = url;
            produto.CdPublicIdImagem = publicId;
            await _context.SaveChangesAsync();

            TempData["Msg"] = $"Imagem do produto {cdProduto} enviada com sucesso.";
        }
        catch (Exception ex)
        {
            TempData["Msg"] = "Erro: " + ex.Message;
        }

        return RedirectToAction(nameof(Imagens));
    }
}