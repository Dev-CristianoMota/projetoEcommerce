using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ViewModels;

public class CadastroViewModel
{
    // 1 = Vendedor, 2 = Comprador (Admin nunca é permitido no cadastro público)
    [Display(Name = "Tipo de conta")]
    [Range(1, 2, ErrorMessage = "Escolha entre Comprador ou Vendedor.")]
    public int CdTipoUsuario { get; set; } = 2;

    [Display(Name = "Nome completo")]
    [Required(ErrorMessage = "Informe seu nome.")]
    [StringLength(150, ErrorMessage = "O nome pode ter no máximo {1} caracteres.")]
    public string NmUsuario { get; set; } = string.Empty;

    [Display(Name = "Apelido")]
    [Required(ErrorMessage = "Informe um apelido.")]
    [StringLength(50, ErrorMessage = "O apelido pode ter no máximo {1} caracteres.")]
    public string NmApelido { get; set; } = string.Empty;

    [Display(Name = "Telefone")]
    [Required(ErrorMessage = "Informe seu telefone com DDD.")]
    [StringLength(20, ErrorMessage = "Telefone inválido.")]
    public string NmTelefone { get; set; } = string.Empty;

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "Informe seu e-mail.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [StringLength(180, ErrorMessage = "O e-mail pode ter no máximo {1} caracteres.")]
    public string NmEmail { get; set; } = string.Empty;

    [Display(Name = "Senha")]
    [Required(ErrorMessage = "Informe uma senha.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo {2} caracteres.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;

    [Display(Name = "Confirmar senha")]
    [Required(ErrorMessage = "Confirme a senha.")]
    [Compare(nameof(Senha), ErrorMessage = "As senhas não conferem.")]
    [DataType(DataType.Password)]
    public string ConfirmarSenha { get; set; } = string.Empty;
}