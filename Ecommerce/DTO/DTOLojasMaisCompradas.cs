namespace Ecommerce.DTO
{
    public class DTOLojasMaisCompradas
    {
        public int CdLojaVirtual { get; set; }
        public string NmLojaVirtual { get; set; } = string.Empty;
        public string? DsLojaVirtual { get; set; }
        public int QtPedidos { get; set; }
    }
}