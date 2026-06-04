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
    public DbSet<ProductNote> ProductNotes => Set<ProductNote>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductNote>()
            .HasOne(productNote => productNote.Product)
            .WithMany(product => product.ProductNotes)
            .HasForeignKey(productNote => productNote.ProductId);
    }
    
}
