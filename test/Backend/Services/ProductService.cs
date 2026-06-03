using Backend.Dtos;
using Backend.Models;
using Backend.Repositories;
using Mapster;

namespace Backend.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

//task es para  que sea asincrono el metodo 
    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Adapt<List<ProductDto>>();
    }

    public async Task<ProductDetailDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product.Adapt<ProductDetailDto?>();
    }

    //task es para  que sea asincrono
    public async Task<List<ProductDto>> SearchByNameAsync(string name)
    {
        var products = await _productRepository.SearchByNameAsync(name);
        return products.Adapt<List<ProductDto>>();
    }

    public async Task<(ProductWriteResult Result, ProductDetailDto? Product)> CreateAsync(CreateProductDto productDto)
    {
        var productNumberExists = await _productRepository.ProductNumberExistsAsync(productDto.ProductNumber);

        if (productNumberExists)
        {
            return (ProductWriteResult.Conflict, null);
        }

        var product = productDto.Adapt<Product>();
        product.SellStartDate = DateTime.UtcNow;
        product.ModifiedDate = DateTime.UtcNow;
        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        return (ProductWriteResult.Success, product.Adapt<ProductDetailDto>());
    }

    public async Task<ProductWriteResult> UpdateAsync(int id, UpdateProductDto productDto)
    {
        var productFromDb = await _productRepository.GetByIdForUpdateAsync(id);

        if (productFromDb is null)
        {
            return ProductWriteResult.NotFound;
        }

        var productNumberExists = await _productRepository.ProductNumberExistsAsync(
            productDto.ProductNumber,
            id);

        if (productNumberExists)
        {
            return ProductWriteResult.Conflict;
        }

        productFromDb.Name = productDto.Name;
        productFromDb.ProductNumber = productDto.ProductNumber;
        productFromDb.Color = productDto.Color;
        productFromDb.SafetyStockLevel = productDto.SafetyStockLevel;
        productFromDb.ReorderPoint = productDto.ReorderPoint;
        productFromDb.StandardCost = productDto.StandardCost;
        productFromDb.ListPrice = productDto.ListPrice;
        productFromDb.Size = productDto.Size;
        productFromDb.Weight = productDto.Weight;
        productFromDb.DaysToManufacture = productDto.DaysToManufacture;
        productFromDb.ModifiedDate = DateTime.UtcNow;

        await _productRepository.SaveChangesAsync();

        return ProductWriteResult.Success;
    }

    public async Task<(ProductWriteResult Result, ProductDetailDto? Product)> PatchAsync(
        int id,
        PatchProductDto productDto)
    {
        var productFromDb = await _productRepository.GetByIdForUpdateAsync(id);

        if (productFromDb is null)
        {
            return (ProductWriteResult.NotFound, null);
        }

        if (!string.IsNullOrWhiteSpace(productDto.Name))
        {
            productFromDb.Name = productDto.Name;
        }

        if (!string.IsNullOrWhiteSpace(productDto.ProductNumber))
        {
            var productNumberExists = await _productRepository.ProductNumberExistsAsync(
                productDto.ProductNumber,
                id);

            if (productNumberExists)
            {
                return (ProductWriteResult.Conflict, null);
            }

            productFromDb.ProductNumber = productDto.ProductNumber;
        }

        if (productDto.Color is not null)
        {
            productFromDb.Color = productDto.Color;
        }

        if (productDto.SafetyStockLevel.HasValue)
        {
            productFromDb.SafetyStockLevel = productDto.SafetyStockLevel.Value;
        }

        if (productDto.ReorderPoint.HasValue)
        {
            productFromDb.ReorderPoint = productDto.ReorderPoint.Value;
        }

        if (productDto.StandardCost.HasValue)
        {
            productFromDb.StandardCost = productDto.StandardCost.Value;
        }

        if (productDto.ListPrice.HasValue)
        {
            productFromDb.ListPrice = productDto.ListPrice.Value;
        }

        if (productDto.Size is not null)
        {
            productFromDb.Size = productDto.Size;
        }

        if (productDto.Weight.HasValue)
        {
            productFromDb.Weight = productDto.Weight.Value;
        }

        if (productDto.DaysToManufacture.HasValue)
        {
            productFromDb.DaysToManufacture = productDto.DaysToManufacture.Value;
        }

        productFromDb.ModifiedDate = DateTime.UtcNow;

        await _productRepository.SaveChangesAsync();

        return (ProductWriteResult.Success, productFromDb.Adapt<ProductDetailDto>());
    }

    public async Task<ProductWriteResult> DeleteAsync(int id)
    {
        var productFromDb = await _productRepository.GetByIdForUpdateAsync(id);

        if (productFromDb is null)
        {
            return ProductWriteResult.NotFound;
        }

        _productRepository.Delete(productFromDb);
        await _productRepository.SaveChangesAsync();

        return ProductWriteResult.Success;
    }
}
