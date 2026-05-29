using Backend.Models;

namespace Backend.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> GetByIdForUpdateAsync(int id);
    Task<List<Product>> SearchByNameAsync(string name);
    Task<bool> ProductNumberExistsAsync(string productNumber, int? excludedProductId = null);
    Task AddAsync(Product product);
    void Delete(Product product);
    Task SaveChangesAsync();
}
