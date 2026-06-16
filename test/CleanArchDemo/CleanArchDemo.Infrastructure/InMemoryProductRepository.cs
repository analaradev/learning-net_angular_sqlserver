using CleanArchDemo.Application;
using CleanArchDemo.Domain;

namespace CleanArchDemo.Infrastructure;

public class InMemoryProductRepository : IProductRepository
{
    private static readonly List<Product> _products = new()
    {
        new() { Id = 1, Name = "Laptop Gamer (Clean)", ListPrice = 1000m },
        new() { Id = 2, Name = "Mouse Inalambrico (Clean)", ListPrice = 50m },
        new() { Id = 3, Name = "Teclado Mecanico (Clean)", ListPrice = 100m }
    };

    public Task<List<Product>> GetAllAsync()
    {
        // Simulamos una consulta asíncrona a la base de datos
        return Task.FromResult(_products);
    }
}
