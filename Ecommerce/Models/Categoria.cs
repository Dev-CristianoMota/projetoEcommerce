using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models;

[Table("categoria")]
[Index("NmCategoria", Name = "uq_categoria_nome", IsUnique = true)]
public partial class Categoria
{
    [Key]
    [Column("cd_categoria")]
    public int CdCategoria { get; set; }

    [Column("nm_categoria")]
    [StringLength(200)]
    public string NmCategoria { get; set; } = null!;

    [InverseProperty("CdCategoriaNavigation")]
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
