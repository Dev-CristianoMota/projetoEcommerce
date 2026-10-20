using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.ViewModels;

public class DadosUsuarioViewModel
{
    [Required(ErrorMessage = "Informe o nome completo.")]
    [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
    [Display(Name = "Nome completo")]
    public string NmUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o apelido.")]
    [StringLength(50, ErrorMessage = "O apelido deve ter no máximo 50 caracteres.")]
    [Display(Name = "Apelido")]
    public string NmApelido { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [StringLength(180, ErrorMessage = "O e-mail deve ter no máximo 180 caracteres.")]
    [Display(Name = "E-mail")]
    public string NmEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o telefone.")]
    [StringLength(15, ErrorMessage = "Telefone inválido.")]
    [Display(Name = "Telefone")]
    public string NmTelefone { get; set; } = string.Empty;

    [BindNever]
    public string? NmTipoUsuario { get; set; }

    [BindNever]
    public DateTime DtCriacao { get; set; }

    public string TelefoneFormatado
    {
        get
        {
            var d = new string((NmTelefone ?? "").Where(char.IsDigit).ToArray());
            return d.Length switch
            {
                11 => $"({d[..2]}) {d[2..7]}-{d[7..]}",
                10 => $"({d[..2]}) {d[2..6]}-{d[6..]}",
                _ => NmTelefone ?? ""
            };
        }
    }
}