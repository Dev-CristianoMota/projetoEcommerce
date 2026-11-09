namespace Ecommerce.DTO
{
    public class DTOResultadoCheckout
    {
        public bool Sucesso { get; set; }
        public string? Erro { get; set; }
        public int CdPedido { get; set; }
    }
}