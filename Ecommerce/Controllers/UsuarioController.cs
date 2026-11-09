using Ecommerce.DTO;
using Ecommerce.Helpers;
using Ecommerce.Models;
using Ecommerce.Services;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Controllers;

public class UsuarioController : BaseController
{
    private readonly UsuarioService _usuarioService;
    private readonly PedidoService _pedidoService;

    public UsuarioController(UsuarioService usuarioService, PedidoService pedidoService)
    {
        _usuarioService = usuarioService;
        _pedidoService = pedidoService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var usuario = await _usuarioService.LoginAsync(model.Email, model.Senha);

        if (usuario is null)
        {
            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            return View(model);
        }

        HttpContext.Session.SetObject(ChaveSessao, usuario);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.CdUsuario.ToString()),
            new(ClaimTypes.Name, usuario.NmUsuario),
            new(ClaimTypes.Email, usuario.NmEmail),
            new(ClaimTypes.Role, usuario.NmTipoUsuario),
            new("apelido", usuario.NmApelido),
            new("cd_tipo_usuario", usuario.CdTipoUsuario.ToString())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    //[Authorize]
    //[HttpGet]
    //public async Task<IActionResult> Conta()
    //{
    //    var logado = UsuarioLogado;

    //    if (logado is null)
    //        return RedirectToAction(nameof(Login));

    //    UltimoPedidoContaViewModel? pedido = await _pedidoService.BuscarUltimoPedidoAsync(logado.CdUsuario);

    //    return View("Conta", pedido);
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Cadastro(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View(new CadastroViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cadastro(CadastroViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var resultado = await _usuarioService.CadastrarAsync(model);

        if (!resultado.Sucesso)
        {
            ModelState.AddModelError(string.Empty, resultado.Erro ?? "Não foi possível criar a conta.");
            return View(model);
        }

        // Já deixa o usuário logado (grava sessão + cookie)
        await AtualizarSessaoAsync(resultado.Usuario!);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> MeusDados()
    {
        var logado = UsuarioLogado;
        if (logado is null)
            return RedirectToAction(nameof(Login));

        var dados = await _usuarioService.ListarDadosAsync(logado.CdUsuario);
        if (dados is null)
            return RedirectToAction(nameof(Login));

        return View(dados);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(DadosUsuarioViewModel model)
    {
        var logado = UsuarioLogado;
        if (logado is null)
            return RedirectToAction(nameof(Login));

        if (!ModelState.IsValid)
            return await VoltarParaEdicaoAsync(logado.CdUsuario, model);

        var resultado = await _usuarioService.EditarAsync(logado.CdUsuario, model);

        if (!resultado.Sucesso)
        {
            ModelState.AddModelError(string.Empty, resultado.Erro ?? "Não foi possível salvar.");
            return await VoltarParaEdicaoAsync(logado.CdUsuario, model);
        }

        await AtualizarSessaoAsync(resultado.Usuario!);

        TempData["Sucesso"] = "Dados atualizados com sucesso!";
        return RedirectToAction(nameof(MeusDados));
    }

    private async Task<IActionResult> VoltarParaEdicaoAsync(int cdUsuario, DadosUsuarioViewModel model)
    {
        var atual = await _usuarioService.ListarDadosAsync(cdUsuario);
        model.NmTipoUsuario = atual?.NmTipoUsuario;
        model.DtCriacao = atual?.DtCriacao ?? default;

        ViewData["Editando"] = true;
        return View("MeusDados", model);
    }

    private async Task AtualizarSessaoAsync(DTOUsuarioLogado usuario)
    {
        HttpContext.Session.SetObject(ChaveSessao, usuario);

        var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, usuario.CdUsuario.ToString()),
        new(ClaimTypes.Name, usuario.NmUsuario),
        new(ClaimTypes.Email, usuario.NmEmail),
        new(ClaimTypes.Role, usuario.NmTipoUsuario),
        new("apelido", usuario.NmApelido),
        new("cd_tipo_usuario", usuario.CdTipoUsuario.ToString())
    };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));
    }
}