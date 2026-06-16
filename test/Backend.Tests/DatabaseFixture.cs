using Backend.Data;
using Backend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Backend.Tests;

public class DatabaseFixture : IDisposable
{
    public AdventureWorksContext Context { get; }

    public DatabaseFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(typeof(AdventureWorksContext).Assembly)
            .Build();

        var connectionString = configuration.GetConnectionString("AdventureWorks");

        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = "AdventureWorks2025_Test"
        };

        var options = new DbContextOptionsBuilder<AdventureWorksContext>()
            .UseSqlServer(connectionStringBuilder.ConnectionString)
            .Options;

        Context = new AdventureWorksContext(options);

        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var products = new List<Product>
        {
            new() { Name = "Laptop Gamer", ProductNumber = "LPT-001", SafetyStockLevel = 10, ReorderPoint = 5, StandardCost = 500m, ListPrice = 800m, SellStartDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new() { Name = "Mouse Inalambrico", ProductNumber = "MS-002", SafetyStockLevel = 50, ReorderPoint = 20, StandardCost = 10m, ListPrice = 20m, SellStartDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new() { Name = "Teclado Mecanico", ProductNumber = "KB-003", SafetyStockLevel = 30, ReorderPoint = 15, StandardCost = 30m, ListPrice = 50m, SellStartDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
        };

        Context.Products.AddRange(products);
        Context.SaveChanges();
        Context.ChangeTracker.Clear();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
