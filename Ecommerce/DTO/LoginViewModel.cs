using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Informe o e-mail ou apelido.")]
    [Display(Name = "E-mail ou Apelido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a senha.")]
    [MinLength(6, ErrorMessage = "Mínimo de 6 caracteres.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;
}