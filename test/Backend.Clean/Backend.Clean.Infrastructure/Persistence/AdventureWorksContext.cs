using Backend.Clean.Application.DTOs;
using Backend.Clean.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Clean.Infrastructure.Persistence;

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
        // ── Product ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product", "Production");

            entity.HasKey(p => p.ProductId);

            entity.Property(p => p.ProductId)
                  .HasColumnName("ProductID");

            entity.Property(p => p.Name)
                  .HasColumnName("Name")
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(p => p.ProductNumber)
                  .HasColumnName("ProductNumber")
                  .HasMaxLength(25)
                  .IsRequired();

            entity.Property(p => p.Color)
                  .HasColumnName("Color")
                  .HasMaxLength(15);

            entity.Property(p => p.SafetyStockLevel)
                  .HasColumnName("SafetyStockLevel");

            entity.Property(p => p.ReorderPoint)
                  .HasColumnName("ReorderPoint");

            entity.Property(p => p.StandardCost)
                  .HasColumnName("StandardCost")
                  .HasColumnType("money");

            entity.Property(p => p.ListPrice)
                  .HasColumnName("ListPrice")
                  .HasColumnType("money");

            entity.Property(p => p.Size)
                  .HasColumnName("Size")
                  .HasMaxLength(5);

            entity.Property(p => p.Weight)
                  .HasColumnName("Weight")
                  .HasColumnType("decimal(8, 2)");

            entity.Property(p => p.DaysToManufacture)
                  .HasColumnName("DaysToManufacture");

            entity.Property(p => p.SellStartDate)
                  .HasColumnName("SellStartDate");

            entity.Property(p => p.SellEndDate)
                  .HasColumnName("SellEndDate");

            entity.Property(p => p.DiscontinuedDate)
                  .HasColumnName("DiscontinuedDate");

            entity.Property(p => p.ModifiedDate)
                  .HasColumnName("ModifiedDate");

            entity.Property(p => p.ProductSubcategoryId)
                  .HasColumnName("ProductSubcategoryID");
        });

        // ── ProductNote ───────────────────────────────────────────────────────
        modelBuilder.Entity<ProductNote>(entity =>
        {
            entity.ToTable("ProductNotes");

            entity.HasKey(pn => pn.ProductNoteId);

            entity.Property(pn => pn.Note)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(pn => pn.ProductId)
                  .HasColumnName("ProductID");

            entity.HasOne(pn => pn.Product)
                  .WithMany(p => p.ProductNotes)
                  .HasForeignKey(pn => pn.ProductId);
        });
    }
}
