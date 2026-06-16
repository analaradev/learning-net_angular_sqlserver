using CleanArchDemo.Domain;

namespace CleanArchDemo.Application;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
}
