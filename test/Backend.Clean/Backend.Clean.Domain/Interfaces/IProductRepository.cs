using Backend.Clean.Domain.Entities;

namespace Backend.Clean.Domain.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetByIdForUpdateAsync(int id);
    Task<List<Product>> SearchByNameAsync(string name);
    Task<List<Product>> AdvancedSearchAsync(
        string? name,
        string? color,
        decimal? minPrice,
        decimal? maxPrice);
    Task<List<(string Color, int ProductCount, decimal AveragePrice)>> GetProductsGroupedByColorAsync();
    Task<bool> ProductHasNotesAsync(int productId);
    Task<bool> AllNotesHaveTextAsync();
    Task<Product?> GetByProductNumberAsync(string productNumber);
    Task<bool> ProductNumberExistsAsync(string productNumber, int? excludedProductId = null);
    Task AddAsync(Product product);
    Task AddNoteAsync(ProductNote productNote);
    void Delete(Product product);
    Task SaveChangesAsync();
    Task<Product?> GetByIdWithNotesAsync(int id);
    Task<List<Product>> GetProductsByMinPriceWithRawSqlAsync(decimal minPrice);
    Task<Product> CreateProductWithNoteInTransactionAsync(Product product, ProductNote productNote);
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<ProductNote>> GetNotesByProductIdAsync(int productId);
    Task<(int trackedNormal, int trackedNoTracking)> GetTrackingComparisonDataAsync();
}
