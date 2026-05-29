using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class ProductService
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

    public async Task<ProductDetailDto> CreateAsync(Product product)
    {
        product.ModifiedDate = DateTime.UtcNow;

        if (product.SellStartDate == default)
        {
            product.SellStartDate = DateTime.UtcNow;
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product.Adapt<ProductDetailDto>();
    }

    public async Task<bool> UpdateAsync(int id, Product product)
    {
        var productFromDb = await _context.Products
            .FirstOrDefaultAsync(existingProduct => existingProduct.ProductId == id);

        if (productFromDb is null)
        {
            return false;
        }

        productFromDb.Name = product.Name;
        productFromDb.ProductNumber = product.ProductNumber;
        productFromDb.Color = product.Color;
        productFromDb.StandardCost = product.StandardCost;
        productFromDb.ListPrice = product.ListPrice;
        productFromDb.Size = product.Size;
        productFromDb.Weight = product.Weight;
        productFromDb.SellStartDate = product.SellStartDate;
        productFromDb.SellEndDate = product.SellEndDate;
        productFromDb.DiscontinuedDate = product.DiscontinuedDate;
        productFromDb.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ProductDetailDto?> PatchAsync(
        int id,
        ProductPatchRequest request)
    {
        var productFromDb = await _context.Products
            .FirstOrDefaultAsync(existingProduct => existingProduct.ProductId == id);

        if (productFromDb is null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            productFromDb.Name = request.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.ProductNumber))
        {
            productFromDb.ProductNumber = request.ProductNumber;
        }

        if (request.Color is not null)
        {
            productFromDb.Color = request.Color;
        }

        if (request.StandardCost.HasValue)
        {
            productFromDb.StandardCost = request.StandardCost.Value;
        }

        if (request.ListPrice.HasValue)
        {
            productFromDb.ListPrice = request.ListPrice.Value;
        }

        if (request.Size is not null)
        {
            productFromDb.Size = request.Size;
        }

        if (request.Weight.HasValue)
        {
            productFromDb.Weight = request.Weight.Value;
        }

        productFromDb.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return productFromDb.Adapt<ProductDetailDto>();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var productFromDb = await _context.Products
            .FirstOrDefaultAsync(existingProduct => existingProduct.ProductId == id);

        if (productFromDb is null)
        {
            return false;
        }

        _context.Products.Remove(productFromDb);
        await _context.SaveChangesAsync();

        return true;
    }
}
