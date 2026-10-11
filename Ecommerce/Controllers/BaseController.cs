using Ecommerce.DTO;
using Ecommerce.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    public abstract class BaseController : Controller
    {
        protected const string ChaveSessao = "UsuarioLogado";
        protected DTOUsuarioLogado? UsuarioLogado
        {
            get
            {
                var dto = HttpContext.Session.GetObject<DTOUsuarioLogado>(ChaveSessao);
                if (dto is not null)
                    return dto;

                if (User.Identity?.IsAuthenticated != true)
                    return null;

                if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var cdUsuario))
                    return null;

                dto = new DTOUsuarioLogado
                {
                    CdUsuario = cdUsuario,
                    NmApelido = User.FindFirstValue("apelido") ?? string.Empty,
                    NmUsuario = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
                    NmEmail = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                    CdTipoUsuario = int.TryParse(User.FindFirstValue("cd_tipo_usuario"), out var tipo) ? tipo : 0,
                    NmTipoUsuario = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty
                };

                HttpContext.Session.SetObject(ChaveSessao, dto);
                return dto;
            }
        }
    }
}