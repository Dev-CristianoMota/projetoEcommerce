using System;
using System.Collections.Generic;

namespace Ecommerce.Models;

public partial class Usuario
{
    public int CdUsuario { get; set; }

    public string NmApelido { get; set; } = null!;

    public string NmUsuario { get; set; } = null!;

    public string? NmTelefone { get; set; } = null!;

    public string NmEmail { get; set; } = null!;

    public string NmSenha { get; set; } = null!;

    public int CdTipoUsuario { get; set; }

    public DateTime DtCriacao { get; set; }

    public DateTime? DtAtualizacao { get; set; }

    public bool IcUsuarioAtivo { get; set; }

    public virtual TipoUsuario CdTipoUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<LojaVirtual> LojaVirtuals { get; set; } = new List<LojaVirtual>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
}
