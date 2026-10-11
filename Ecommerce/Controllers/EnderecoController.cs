using Ecommerce.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    [Authorize]
    [Route("Usuario/Conta")]
    public class EnderecoController : Controller
    {
        private readonly EnderecoService _enderecoService;

        public EnderecoController(EnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }

        private int ObterUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("")]
        public IActionResult Conta() => View("~/Views/Usuario/Conta.cshtml");

        [HttpGet("Endereco")]
        public async Task<IActionResult> Endereco()
        {
            var enderecos = await _enderecoService.ObterEnderecosPorClienteAsync(ObterUsuarioId());
            return View("~/Views/Usuario/Endereco.cshtml", enderecos);
        }
    }
}