using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockManager.Domain.Entities;

namespace StockManager.Infrastructure.Persistence.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.Property(x => x.NomeCategoria).HasMaxLength(100);
        builder.HasIndex(x => x.NomeCategoria).IsUnique();
        builder.Property(x => x.StatusCategoria).HasConversion<string>().HasMaxLength(20);
    }

}
