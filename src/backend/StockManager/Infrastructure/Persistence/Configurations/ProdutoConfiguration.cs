using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockManager.Domain.Entities;

namespace StockManager.Infrastructure.Persistence.Configurations;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.Property(x => x.NomeProduto).HasMaxLength(100);
        builder.Property(x => x.DescProduto).HasMaxLength(500);
        builder.Property(x => x.Sku).HasMaxLength(20);
        builder.HasIndex(x => x.Sku).IsUnique();
        builder.Property(x => x.UnidadeMedida).HasConversion<string>().HasMaxLength(10);
        builder.Property(x => x.StatusProduto).HasConversion<string>().HasMaxLength(20);

        builder.ToTable(table => table.HasCheckConstraint("CK_Produto_EstoqueMinimo", "EstoqueMinimo >= 0"));

        builder.HasOne(x => x.Categoria)
            .WithMany()
            .HasForeignKey(x => x.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}