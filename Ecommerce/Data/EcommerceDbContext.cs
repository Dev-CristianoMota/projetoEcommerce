using System;
using System.Collections.Generic;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data;

public partial class EcommerceDbContext : DbContext
{
    public EcommerceDbContext()
    {
    }

    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options): base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Cupom> Cupoms { get; set; }

    public virtual DbSet<ItemPedido> ItemPedidos { get; set; }

    public virtual DbSet<LojaVirtual> LojaVirtuals { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=db_ecommerce;user=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.43-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CdCategoria).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.HasIndex(e => e.NmCategoria, "uq_categoria_nome").IsUnique();

            entity.Property(e => e.CdCategoria).HasColumnName("cd_categoria");
            entity.Property(e => e.NmCategoria)
                .HasMaxLength(200)
                .HasColumnName("nm_categoria");
        });

        modelBuilder.Entity<Cupom>(entity =>
        {
            entity.HasKey(e => e.CdCupom).HasName("PRIMARY");

            entity.ToTable("cupom");

            entity.HasIndex(e => e.CdPromocional, "uq_cupom_promocional").IsUnique();

            entity.Property(e => e.CdCupom).HasColumnName("cd_cupom");
            entity.Property(e => e.CdPromocional)
                .HasMaxLength(45)
                .HasColumnName("cd_promocional");
            entity.Property(e => e.DtFimValidade)
                .HasColumnType("datetime")
                .HasColumnName("dt_fim_validade");
            entity.Property(e => e.DtInicioValidade)
                .HasColumnType("datetime")
                .HasColumnName("dt_inicio_validade");
            entity.Property(e => e.IcCupomAtivo)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("ic_cupom_ativo");
            entity.Property(e => e.SgTipoDesconto)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("sg_tipo_desconto");
            entity.Property(e => e.VlDesconto)
                .HasPrecision(10, 2)
                .HasColumnName("vl_desconto");
        });

        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.HasKey(e => e.CdItemPedido).HasName("PRIMARY");

            entity.ToTable("item_pedido");

            entity.HasIndex(e => e.CdProduto, "fk_item_pedido_produto");

            entity.HasIndex(e => new { e.CdPedido, e.CdProduto }, "uq_item_pedido_produto").IsUnique();

            entity.Property(e => e.CdItemPedido).HasColumnName("cd_item_pedido");
            entity.Property(e => e.CdPedido).HasColumnName("cd_pedido");
            entity.Property(e => e.CdProduto).HasColumnName("cd_produto");
            entity.Property(e => e.QtItem).HasColumnName("qt_item");
            entity.Property(e => e.VlPrecoUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("vl_preco_unitario");

            entity.HasOne(d => d.CdPedidoNavigation).WithMany(p => p.ItemPedidos)
                .HasForeignKey(d => d.CdPedido)
                .HasConstraintName("fk_item_pedido_pedido");

            entity.HasOne(d => d.CdProdutoNavigation).WithMany(p => p.ItemPedidos)
                .HasForeignKey(d => d.CdProduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_pedido_produto");
        });

        modelBuilder.Entity<LojaVirtual>(entity =>
        {
            entity.HasKey(e => e.CdLojaVirtual).HasName("PRIMARY");

            entity.ToTable("loja_virtual");

            entity.HasIndex(e => e.CdUsuario, "fk_loja_virtual_usuario");

            entity.HasIndex(e => e.NmLojaVirtual, "uq_loja_virtual_nome").IsUnique();

            entity.Property(e => e.CdLojaVirtual).HasColumnName("cd_loja_virtual");
            entity.Property(e => e.CdUsuario).HasColumnName("cd_usuario");
            entity.Property(e => e.DsLojaVirtual)
                .HasColumnType("text")
                .HasColumnName("ds_loja_virtual");
            entity.Property(e => e.DtCriacao)
                .HasColumnType("datetime")
                .HasColumnName("dt_criacao");
            entity.Property(e => e.IcLojaAtiva)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("ic_loja_ativa");
            entity.Property(e => e.NmLojaVirtual)
                .HasMaxLength(200)
                .HasColumnName("nm_loja_virtual");

            entity.HasOne(d => d.CdUsuarioNavigation).WithMany(p => p.LojaVirtuals)
                .HasForeignKey(d => d.CdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_loja_virtual_usuario");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.CdPedido).HasName("PRIMARY");

            entity.ToTable("pedido");

            entity.HasIndex(e => e.CdCupom, "fk_pedido_cupom");

            entity.HasIndex(e => e.CdLojaVirtual, "fk_pedido_loja_virtual");

            entity.HasIndex(e => e.CdUsuario, "fk_pedido_usuario");

            entity.Property(e => e.CdPedido).HasColumnName("cd_pedido");
            entity.Property(e => e.CdCupom).HasColumnName("cd_cupom");
            entity.Property(e => e.CdLojaVirtual).HasColumnName("cd_loja_virtual");
            entity.Property(e => e.CdUsuario).HasColumnName("cd_usuario");
            entity.Property(e => e.DtAtualizacaoPedido)
                .HasColumnType("datetime")
                .HasColumnName("dt_atualizacao_pedido");
            entity.Property(e => e.DtCriacaoPedido)
                .HasColumnType("datetime")
                .HasColumnName("dt_criacao_pedido");
            entity.Property(e => e.SgStatusPedido)
                .HasMaxLength(1)
                .HasDefaultValueSql("'P'")
                .IsFixedLength()
                .HasColumnName("sg_status_pedido");
            entity.Property(e => e.VlDescontoPedido)
                .HasPrecision(10, 2)
                .HasColumnName("vl_desconto_pedido");
            entity.Property(e => e.VlSubtotalPedido)
                .HasPrecision(10, 2)
                .HasColumnName("vl_subtotal_pedido");
            entity.Property(e => e.VlTotalPedido)
                .HasPrecision(10, 2)
                .HasColumnName("vl_total_pedido");

            entity.HasOne(d => d.CdCupomNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.CdCupom)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_pedido_cupom");

            entity.HasOne(d => d.CdLojaVirtualNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.CdLojaVirtual)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_pedido_loja_virtual");

            entity.HasOne(d => d.CdUsuarioNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.CdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_pedido_usuario");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.CdProduto).HasName("PRIMARY");

            entity.ToTable("produto");

            entity.HasIndex(e => e.CdCategoria, "fk_produto_categoria");

            entity.HasIndex(e => e.CdLojaVirtual, "fk_produto_loja_virtual");

            entity.Property(e => e.CdProduto).HasColumnName("cd_produto");
            entity.Property(e => e.CdCategoria).HasColumnName("cd_categoria");
            entity.Property(e => e.CdLojaVirtual).HasColumnName("cd_loja_virtual");
            entity.Property(e => e.DsProduto)
                .HasColumnType("text")
                .HasColumnName("ds_produto");
            entity.Property(e => e.DtAtualizacao)
                .HasColumnType("datetime")
                .HasColumnName("dt_atualizacao");
            entity.Property(e => e.DtCriacao)
                .HasColumnType("datetime")
                .HasColumnName("dt_criacao");
            entity.Property(e => e.IcProdutoAtivo)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("ic_produto_ativo");
            entity.Property(e => e.NmProduto)
                .HasMaxLength(200)
                .HasColumnName("nm_produto");
            entity.Property(e => e.QtdEstoque).HasColumnName("qt_estoque");
            entity.Property(e => e.VlProduto)
                .HasPrecision(10, 2)
                .HasColumnName("vl_produto");

            entity.HasOne(d => d.CdCategoriaNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.CdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produto_categoria");

            entity.HasOne(d => d.CdLojaVirtualNavigation).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.CdLojaVirtual)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produto_loja_virtual");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.CdTipoUsuario).HasName("PRIMARY");

            entity.ToTable("tipo_usuario");

            entity.HasIndex(e => e.NmTipoUsuario, "uq_tipo_usuario_nome").IsUnique();

            entity.Property(e => e.CdTipoUsuario).HasColumnName("cd_tipo_usuario");
            entity.Property(e => e.NmTipoUsuario)
                .HasMaxLength(50)
                .HasColumnName("nm_tipo_usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.CdUsuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.CdTipoUsuario, "fk_usuario_tipo_usuario");

            entity.HasIndex(e => e.NmApelido, "uq_usuario_apelido").IsUnique();

            entity.HasIndex(e => e.NmEmail, "uq_usuario_email").IsUnique();

            entity.HasIndex(e => e.NmTelefone, "uq_usuario_telefone").IsUnique();

            entity.Property(e => e.CdUsuario).HasColumnName("cd_usuario");
            entity.Property(e => e.CdTipoUsuario).HasColumnName("cd_tipo_usuario");
            entity.Property(e => e.DtCriacao)
                .HasColumnType("datetime")
                .HasColumnName("dt_criacao");
            entity.Property(e => e.DtAtualizacao)
                .HasColumnType("datetime")
                .HasColumnName("dt_atualizacao");
            entity.Property(e => e.NmApelido)
                .HasMaxLength(50)
                .HasColumnName("nm_apelido");
            entity.Property(e => e.NmEmail)
                .HasMaxLength(180)
                .HasColumnName("nm_email");
            entity.Property(e => e.NmSenha)
                .HasMaxLength(255)
                .HasColumnName("nm_senha");
            entity.Property(e => e.NmTelefone)
                .HasMaxLength(11)
                .HasColumnName("nm_telefone");
            entity.Property(e => e.NmUsuario)
                .HasMaxLength(150)
                .HasColumnName("nm_usuario");
            entity.Property(e => e.IcUsuarioAtivo)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("ic_usuario_ativo");

            entity.HasOne(d => d.CdTipoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.CdTipoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_tipo_usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
