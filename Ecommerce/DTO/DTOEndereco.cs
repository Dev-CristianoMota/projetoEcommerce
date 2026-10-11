using Ecommerce.Models;

namespace Ecommerce.DTO
{
    public class DTOEndereco
    {
        public int CdEndereco { get; set; }

        public string NmIdentificacao { get; set; } = null!;

        public string NmDestinatario { get; set; } = null!;

        public string CEP { get; set; } = null!;

        public string Logradouro { get; set; } = null!;

        public string Numero { get; set; } = null!;

        public string? DsComplemento { get; set; }

        public string Bairro { get; set; } = null!;

        public string Cidade { get; set; } = null!;

        public string UF { get; set; } = null!;

        public bool IcEnderecoPrincipal { get; set; }
    }
}
