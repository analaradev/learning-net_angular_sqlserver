using Backend.Data;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests;

public class ProductRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly AdventureWorksContext _context;
    private readonly ProductRepository _repository;

    public ProductRepositoryIntegrationTests(DatabaseFixture fixture)
    {
        _context = fixture.Context;
        _repository = new ProductRepository(_context);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsSeededProductsOrderedByName()
    {
        var products = await _repository.GetAllAsync();

        Assert.NotNull(products);
        Assert.Equal(3, products.Count);
        Assert.Equal("Laptop Gamer", products[0].Name);
        Assert.Equal("Mouse Inalambrico", products[1].Name);
        Assert.Equal("Teclado Mecanico", products[2].Name);
    }

    [Fact]
    public async Task Delete_WhenProductExists_RemovesProductFromDatabase()
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var products = await _repository.GetAllAsync();
            var mouse = products.FirstOrDefault(p => p.ProductNumber == "MS-002");
            Assert.NotNull(mouse);

            _repository.Delete(mouse);
            await _repository.SaveChangesAsync();

            var deletedProduct = await _repository.GetByIdAsync(mouse.ProductId);
            Assert.Null(deletedProduct);

            var remainingProducts = await _repository.GetAllAsync();
            Assert.Equal(2, remainingProducts.Count);
        }
        finally
        {
            await transaction.RollbackAsync();
            _context.ChangeTracker.Clear();
        }
    }
}
