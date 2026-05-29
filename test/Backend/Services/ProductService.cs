using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class ProductService : IProductService
{
    private readonly AdventureWorksContext _context;

    public ProductService(AdventureWorksContext context)
    {
        _context = context;
    }

//task es para  que sea asincrono el metodo 
    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _context.Products
            .AsNoTracking() // este es solo se usa para lectura
            .OrderBy(product => product.Name)
            .Take(10)
            .ToListAsync();


        return products.Adapt<List<ProductDto>>();
    }

    public async Task<ProductDetailDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Where(product => product.ProductId == id)
            .FirstOrDefaultAsync();

        return product.Adapt<ProductDetailDto?>();
    }

    //task es para  que sea asincrono
    public async Task<List<ProductDto>> SearchByNameAsync(string name)
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(product => product.Name.Contains(name))
            .OrderBy(product => product.Name)
            .Take(20)
            .ToListAsync();


        return products.Adapt<List<ProductDto>>();
    }

    public async Task<(ProductWriteResult Result, ProductDetailDto? Product)> CreateAsync(CreateProductDto productDto)
    {
        var productNumberExists = await _context.Products
            .AnyAsync(product => product.ProductNumber == productDto.ProductNumber);

        if (productNumberExists)
        {
            return (ProductWriteResult.Conflict, null);
        }

        var product = productDto.Adapt<Product>();
        product.SellStartDate = DateTime.UtcNow;
        product.ModifiedDate = DateTime.UtcNow;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return (ProductWriteResult.Success, product.Adapt<ProductDetailDto>());
    }

    public async Task<ProductWriteResult> UpdateAsync(int id, UpdateProductDto productDto)
    {
        var productFromDb = await _context.Products
            .FirstOrDefaultAsync(existingProduct => existingProduct.ProductId == id);

        if (productFromDb is null)
        {
            return ProductWriteResult.NotFound;
        }

        var productNumberExists = await _context.Products
            .AnyAsync(product =>
                product.ProductId != id &&
                product.ProductNumber == productDto.ProductNumber);

        if (productNumberExists)
        {
            return ProductWriteResult.Conflict;
        }

        productFromDb.Name = productDto.Name;
        productFromDb.ProductNumber = productDto.ProductNumber;
        productFromDb.Color = productDto.Color;
        productFromDb.StandardCost = productDto.StandardCost;
        productFromDb.ListPrice = productDto.ListPrice;
        productFromDb.Size = productDto.Size;
        productFromDb.Weight = productDto.Weight;
        productFromDb.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ProductWriteResult.Success;
    }

    public async Task<(ProductWriteResult Result, ProductDetailDto? Product)> PatchAsync(
        int id,
        PatchProductDto productDto)
    {
        var productFromDb = await _context.Products
            .FirstOrDefaultAsync(existingProduct => existingProduct.ProductId == id);

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
            var productNumberExists = await _context.Products
                .AnyAsync(product =>
                    product.ProductId != id &&
                    product.ProductNumber == productDto.ProductNumber);

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

        productFromDb.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return (ProductWriteResult.Success, productFromDb.Adapt<ProductDetailDto>());
    }

    public async Task<ProductWriteResult> DeleteAsync(int id)
    {
        var productFromDb = await _context.Products
            .FirstOrDefaultAsync(existingProduct => existingProduct.ProductId == id);

        if (productFromDb is null)
        {
            return ProductWriteResult.NotFound;
        }

        _context.Products.Remove(productFromDb);
        await _context.SaveChangesAsync();

        return ProductWriteResult.Success;
    }
}
