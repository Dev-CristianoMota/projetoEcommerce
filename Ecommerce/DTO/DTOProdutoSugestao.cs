namespace Ecommerce.DTO;

public class DTOProdutoSugestao
{
    public int CdProduto { get; set; }
    public string NmProduto { get; set; } = "";
    public decimal VlProduto { get; set; }
    public string? NmUrlImagem { get; set; }
}