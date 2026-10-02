namespace Ecommerce.Models.DTO
{
    public class DTOCategoriaMaisVendida
    {
        public int CdCategoria { get; set; }
        public string NmCategoria { get; set; } = string.Empty;
        public int QtdVendida { get; set; }
        public int QtdProdutosVendidos { get; set; }
    }
}
