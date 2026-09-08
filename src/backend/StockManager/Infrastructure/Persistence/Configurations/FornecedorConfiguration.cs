using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockManager.Domain.Entities;

namespace StockManager.Infrastructure.Persistence.Configurations;

public class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
{
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
        builder.Property(x => x.NomeFornecedor).HasMaxLength(100);
        builder.Property(x => x.TelFornecedor).HasMaxLength(20);
        builder.Property(x => x.CnpjFornecedor).HasMaxLength(20);
        builder.HasIndex(x => x.CnpjFornecedor).IsUnique();
        builder.Property(x => x.Logradouro).HasMaxLength(100);
        builder.Property(x => x.Bairro).HasMaxLength(20);
        builder.Property(x => x.NumeroEmpresa).HasMaxLength(20);
        builder.Property(x => x.Cidade).HasMaxLength(50);
        builder.Property(x => x.Estado).HasMaxLength(2);
        builder.Property(x => x.Cep).HasMaxLength(9);
        builder.Property(x => x.Complemento).HasMaxLength(50);
        builder.Property(x => x.StatusFornecedor).HasConversion<string>().HasMaxLength(20);
    }
}