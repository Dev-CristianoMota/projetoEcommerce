using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Endereco
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

        public bool IcEnderecoAtivo { get; set; } = true;

        public DateTime DtCriacao { get; set; }

        public DateTime? DtAtualizacao { get; set; }

        public int CdUsuario { get; set; }

        public virtual Usuario CdUsuarioNavigation { get; set; } = null!;

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}    