using System;
using System.Collections.Generic;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data;

public partial class EcommerceDbContext : DbContext
{
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options): base(options)
    {

    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cupom> Cupons { get; set; }

    public virtual DbSet<ItemPedido> ItemPedidos { get; set; }

    public virtual DbSet<LojaVirtual> LojasVirtuais { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CdCategoria).HasName("PRIMARY");
        });

        modelBuilder.Entity<Cupom>(entity =>
        {
            entity.HasKey(e => e.CdCupom).HasName("PRIMARY");

            entity.Property(e => e.IcCupomAtivo).HasDefaultValueSql("'1'");
            entity.Property(e => e.SgTipoDesconto).IsFixedLength();
        });

        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.HasKey(e => e.CdItemPedido).HasName("PRIMARY");

            entity.HasOne(d => d.CdPedidoNavigation).WithMany(p => p.ItemPedidos).HasConstraintName("fk_item_pedido_pedido");

            entity.HasOne(d => d.CdProdutoNavigation).WithMany(p => p.ItemPedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_pedido_produto");
        });

        modelBuilder.Entity<LojaVirtual>(entity =>
        {
            entity.HasKey(e => e.CdLojaVirtual).HasName("PRIMARY");

            entity.Property(e => e.DtCriacao).HasDefaultValueSql("CURRENT_TIMESTAMP(4)");
            entity.Property(e => e.IcLojaAtiva).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.CdUsuarioNavigation).WithMany(p => p.LojaVirtuals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_loja_virtual_usuario");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.CdPedido).HasName("PRIMARY");

            entity.Property(e => e.DtAtualizacaoPedido).ValueGeneratedOnAddOrUpdate();
            entity.Property(e => e.DtCriacaoPedido).HasDefaultValueSql("CURRENT_TIMESTAMP(4)");
            entity.Property(e => e.SgStatusPedido)
                .HasDefaultValueSql("'P'")
                .IsFixedLength();

            entity.HasOne(d => d.CdCupomNavigation).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_pedido_cupom");

            entity.HasOne(d => d.CdLojaVirtualNavigation).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_pedido_loja_virtual");

            entity.HasOne(d => d.CdUsuarioNavigation).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_pedido_usuario");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.CdProduto).HasName("PRIMARY");

            entity.Property(e => e.DtAtualizacao).ValueGeneratedOnAddOrUpdate();
            entity.Property(e => e.DtCriacao).HasDefaultValueSql("CURRENT_TIMESTAMP(4)");
            entity.Property(e => e.IcProdutoAtivo).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.CdCategoriaNavigation).WithMany(p => p.Produtos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produto_categoria");

            entity.HasOne(d => d.CdLojaVirtualNavigation).WithMany(p => p.Produtos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produto_loja_virtual");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.CdTipoUsuario).HasName("PRIMARY");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.CdUsuario).HasName("PRIMARY");

            entity.Property(e => e.DtCriacao).HasDefaultValueSql("CURRENT_TIMESTAMP(4)");

            entity.HasOne(d => d.CdTipoUsuarioNavigation).WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_tipo_usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
