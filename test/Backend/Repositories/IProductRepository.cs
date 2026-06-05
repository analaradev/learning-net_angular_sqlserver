using Backend.Dtos;
using Backend.Models;

namespace Backend.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetByIdForUpdateAsync(int id);
    Task<List<Product>> SearchByNameAsync(string name);
    Task<List<ProductAdvancedSearchDto>> AdvancedSearchAsync(
        string? name,
        string? color,
        decimal? minPrice,
        decimal? maxPrice);
    Task<List<ProductColorGroupDto>> GetProductsGroupedByColorAsync();
    Task<List<ProductNote>> GetNotesByProductIdAsync(int productId);
    Task<bool> ProductHasNotesAsync(int productId);
    Task<bool> AllNotesHaveTextAsync();
    Task<Product?> GetByProductNumberAsync(string productNumber);
    Task<TrackingComparisonDto> GetTrackingComparisonAsync();
    Task<bool> ProductNumberExistsAsync(string productNumber, int? excludedProductId = null);
    Task AddAsync(Product product);
    Task AddNoteAsync(ProductNote productNote);
    void Delete(Product product);
    Task SaveChangesAsync();
    Task<Product?> GetByIdWithNotesAsync(int id);
}
