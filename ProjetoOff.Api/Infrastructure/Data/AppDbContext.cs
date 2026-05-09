using Microsoft.EntityFrameworkCore;
using ProjetoOff.Api.Features.Products;
using ProjetoOff.Api.Features.Clients;
using ProjetoOff.Api.Features.Suppliers;
using ProjetoOff.Api.Features.Ufs;
using ProjetoOff.Api.Features.Cnaes;
namespace ProjetoOff.Api.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Uf> Ufs => Set<Uf>();
    public DbSet<Cnae> Cnaes => Set<Cnae>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Product configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Price).HasPrecision(18, 2);
        });

        // Client configuration
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Cnpj).IsRequired().HasMaxLength(14);
        });

        // Supplier configuration
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.RazaoSocial).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Cnpj).IsRequired().HasMaxLength(14);
        });

        // Uf configuration
        modelBuilder.Entity<Uf>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(2);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
        });

        // Cnae configuration
        modelBuilder.Entity<Cnae>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
        });
    }
}
