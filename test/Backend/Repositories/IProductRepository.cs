using Backend.Models;

namespace Backend.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetByIdForUpdateAsync(int id);
    Task<List<Product>> SearchByNameAsync(string name);
    Task<List<ProductNote>> GetNotesByProductIdAsync(int productId);
    Task<bool> ProductNumberExistsAsync(string productNumber, int? excludedProductId = null);
    Task AddAsync(Product product);
    Task AddNoteAsync(ProductNote productNote);
    void Delete(Product product);
    Task SaveChangesAsync();
    Task<Product?> GetByIdWithNotesAsync(int id);
}
