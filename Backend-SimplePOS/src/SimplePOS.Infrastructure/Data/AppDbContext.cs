//Backend-SimplePOS/src/SimplePOS.Infraestructure/Data/AppDbContext.cs

using Microsoft.EntityFrameworkCore;
using SimplePOS.Domain.Entities;

namespace SimplePOS.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    public DbSet<Tenant> Tenants { get; set; } 
    public DbSet<Branch> Branches { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductGroup> ProductGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("citext");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}