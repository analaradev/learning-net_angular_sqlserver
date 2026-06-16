using Backend.Dtos;
using Backend.Models;
using Backend.Repositories;
using Backend.Services;
using Moq;
using Xunit;

namespace Backend.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductDoesNotExist_ReturnsNotFound()
    {
        _productRepositoryMock
            .Setup(repo => repo.GetByIdForUpdateAsync(99))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _productService.DeleteAsync(99);

        // Assert
        Assert.Equal(ProductWriteResult.NotFound, result);
        
        // Verificamos que no se llamara a Delete ni a SaveChangesAsync
        _productRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Product>()), Times.Never);
        _productRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductExists_DeletesProductAndReturnsSuccess()
    {
        // Arrange
        int productId = 1;
        var product = new Product 
        { 
            ProductId = productId, 
            Name = "Test Product",
            ProductNumber = "TEST-001"
        };
        
        _productRepositoryMock
            .Setup(repo => repo.GetByIdForUpdateAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _productService.DeleteAsync(productId);

        // Assert
        Assert.Equal(ProductWriteResult.Success, result);
        
        // Verificamos que se llamara a Delete y a SaveChangesAsync exactamente una vez
        _productRepositoryMock.Verify(repo => repo.Delete(product), Times.Once);
        _productRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}
