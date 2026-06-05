using Backend.Data;
using Backend.Dtos;
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

    public async Task<List<ProductAdvancedSearchDto>> AdvancedSearchAsync(
        string? name,
        string? color,
        decimal? minPrice,
        decimal? maxPrice)
    {
        var query = _context.Products
            .AsNoTracking()
            .Include(product => product.ProductNotes)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(product => product.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(color))
        {
            query = query.Where(product => product.Color == color);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(product => product.ListPrice >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(product => product.ListPrice <= maxPrice.Value);
        }

        return await query
            .OrderByDescending(product => product.ListPrice)
            .Take(20)
            .Select(product => new ProductAdvancedSearchDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                ProductNumber = product.ProductNumber,
                Color = product.Color,
                ListPrice = product.ListPrice,
                NotesCount = product.ProductNotes.Count
            })
            .ToListAsync();
    }

    public async Task<List<ProductColorGroupDto>> GetProductsGroupedByColorAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Where(product => product.Color != null)
            .GroupBy(product => product.Color!)
            .Select(group => new ProductColorGroupDto
            {
                Color = group.Key,
                ProductCount = group.Count(),
                AveragePrice = group.Average(product => product.ListPrice)
            })
            .OrderByDescending(group => group.ProductCount)
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

    public async Task<bool> ProductHasNotesAsync(int productId)
    {
        return await _context.ProductNotes
            .AsNoTracking()
            .AnyAsync(productNote => productNote.ProductId == productId);
    }

    public async Task<bool> AllNotesHaveTextAsync()
    {
        return await _context.ProductNotes
            .AsNoTracking()
            .AllAsync(productNote => productNote.Note != "");
    }

    public async Task<Product?> GetByProductNumberAsync(string productNumber)
    {
        return await _context.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(product => product.ProductNumber == productNumber);
    }

    public async Task<TrackingComparisonDto> GetTrackingComparisonAsync()
    {
        _context.ChangeTracker.Clear();

        await _context.Products
            .Take(5)
            .ToListAsync();

        var trackedEntitiesAfterNormalQuery = _context.ChangeTracker.Entries().Count();

        _context.ChangeTracker.Clear();

        await _context.Products
            .AsNoTracking()
            .Take(5)
            .ToListAsync();

        var trackedEntitiesAfterNoTrackingQuery = _context.ChangeTracker.Entries().Count();

        return new TrackingComparisonDto
        {
            TrackedEntitiesAfterNormalQuery = trackedEntitiesAfterNormalQuery,
            TrackedEntitiesAfterNoTrackingQuery = trackedEntitiesAfterNoTrackingQuery
        };
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
