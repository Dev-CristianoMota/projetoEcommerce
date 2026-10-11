using Ecommerce.Helpers;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Controllers;

public class UsuarioController : BaseController
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
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

        // model.Email recebe o e-mail OU o apelido digitado
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Conta()
    {
        var logado = UsuarioLogado;
        if (logado is null)
            return RedirectToAction(nameof(Login));

        var usuario = await _usuarioService.BuscarPorIdAsync(logado.CdUsuario);
        if (usuario is null)
            return RedirectToAction(nameof(Login));

        return View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}