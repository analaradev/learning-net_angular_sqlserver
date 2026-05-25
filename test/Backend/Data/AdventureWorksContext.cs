using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class AdventureWorksContext : DbContext
{
    public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    //
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Product>(entity =>
    //     {
    //         entity.ToTable("Product", "Production");
    //
    //         entity.HasKey(product => product.ProductId);
    //
    //         entity.Property(product => product.ProductId)
    //             .HasColumnName("ProductID");
    //
    //         entity.Property(product => product.Name)
    //             .HasMaxLength(50);
    //
    //         entity.Property(product => product.ProductNumber)
    //             .HasMaxLength(25);
    //
    //         entity.Property(product => product.Color)
    //             .HasMaxLength(15);
    //
    //         entity.Property(product => product.StandardCost)
    //             .HasColumnType("money");
    //
    //         entity.Property(product => product.ListPrice)
    //             .HasColumnType("money");
    //
    //         entity.Property(product => product.Size)
    //             .HasMaxLength(5);
    //
    //         entity.Property(product => product.Weight)
    //             .HasColumnType("decimal(8, 2)");
    //     });
    // }
}
