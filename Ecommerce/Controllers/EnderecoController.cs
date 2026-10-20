using Ecommerce.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ecommerce.DTO;

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

        [HttpPost("Endereco/TornarPrincipal")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TornarEnderecoPrincipal(int cdEndereco)
        {
            var cdUsuario = ObterUsuarioId();
            var resultado = await _enderecoService.TornarEnderecoPrincipalAsync(cdEndereco, cdUsuario);

            if (!resultado)
            {
                return BadRequest(new { resultado = false, mensagem = "Não foi possível definir o endereço como principal." });
            }

            return Ok(new { resultado = true });
        }

        [HttpPost("Endereco/Cadastrar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastrarEndereco(DTOEndereco dto)
        {
            if (!ModelState.IsValid)
            {
                var mensagem = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                return BadRequest(new { mensagem = mensagem ?? "Dados inválidos." });
            }

            var (sucesso, erro, _) = await _enderecoService.CriarEnderecoAsync(dto, ObterUsuarioId());

            if (!sucesso)
            {
                return BadRequest(new { mensagem = erro });
            }

            return Ok(new { resultado = true });
        }

        [HttpPost("Endereco/Editar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarEndereco(DTOEndereco dto)
        {
            if (!ModelState.IsValid)
            {
                var mensagem = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                return BadRequest(new { mensagem = mensagem ?? "Dados inválidos." });
            }

            var resultado = await _enderecoService.AtualizarEnderecoAsync(dto.CdEndereco, ObterUsuarioId(), dto);

            if (resultado == null)
            {
                return NotFound(new { mensagem = "Endereço não encontrado." });
            }

            return Ok(new { resultado = true });
        }

        [HttpPost("Endereco/Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirEndereco(int cdEndereco)
        {
            try
            {
                var resultado = await _enderecoService.DeletarEnderecoAsync(cdEndereco, ObterUsuarioId());

                if (!resultado)
                {
                    return NotFound(new { mensagem = "Endereço não encontrado." });
                }

                return Ok(new { resultado = true });
            }
            catch (DbUpdateException)
            {
                return Conflict(new { mensagem = "Não foi possível excluir: este endereço está vinculado a outros registros." });
            }
        }
    }
}