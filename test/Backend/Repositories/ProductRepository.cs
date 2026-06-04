using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AdventureWorksContext _context;

    public ProductRepository(AdventureWorksContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .OrderBy(product => product.Name)
            .Take(10)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.ProductId == id);
    }

    public async Task<Product?> GetByIdForUpdateAsync(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(product => product.ProductId == id);
    }

    public async Task<List<Product>> SearchByNameAsync(string name)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(product => product.Name.Contains(name))
            .OrderBy(product => product.Name)
            .Take(20)
            .ToListAsync();
    }

    public async Task<List<ProductNote>> GetNotesByProductIdAsync(int productId)
    {
        return await _context.ProductNotes
            .AsNoTracking()
            .Where(productNote => productNote.ProductId == productId)
            .OrderByDescending(productNote => productNote.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ProductNumberExistsAsync(string productNumber, int? excludedProductId = null)
    {
        return await _context.Products
            .AnyAsync(product =>
                product.ProductNumber == productNumber &&
                (!excludedProductId.HasValue || product.ProductId != excludedProductId.Value));
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task AddNoteAsync(ProductNote productNote)
    {
        await _context.ProductNotes.AddAsync(productNote);
    }

    public void Delete(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Product?> GetByIdWithNotesAsync(int id)
{
    return await _context.Products
        .AsNoTracking()
        .Include(product => product.ProductNotes)
        .FirstOrDefaultAsync(product => product.ProductId == id);
}
}
