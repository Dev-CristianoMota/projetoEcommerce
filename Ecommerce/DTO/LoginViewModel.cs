using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTO;

public class LoginViewModel
{
    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a senha.")]
    [MinLength(6, ErrorMessage = "Mínimo de 6 caracteres.")]
    public string Senha { get; set; } = string.Empty;
}