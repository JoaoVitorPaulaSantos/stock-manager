using Microsoft.EntityFrameworkCore;
using StockManager.Domain.Entities;
using StockManager.Infrastructure.Persistence.Configurations;

namespace StockManager.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base (options)
    {
        
    }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoriaConfiguration).Assembly);
    }
}
