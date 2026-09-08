using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockManager.Domain.Entities;

namespace StockManager.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.Property(x => x.NomeUsuario).HasMaxLength(100);
        builder.Property(x => x.Cpf).HasMaxLength(15);
        builder.HasIndex(x => x.Cpf).IsUnique();
        builder.Property(x => x.Email).HasMaxLength(100);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.HashSenha).HasMaxLength(255);
        builder.Property(x => x.StatusUsuario).HasConversion<string>().HasMaxLength(20);
    }
}