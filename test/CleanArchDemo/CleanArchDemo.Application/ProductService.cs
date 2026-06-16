using CleanArchDemo.Domain;

namespace CleanArchDemo.Application;

public class ProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Product>> GetDiscountedProductsAsync()
    {
        var products = await _repository.GetAllAsync();
        
        // Regla de negocio: Aplicar un 10% de descuento ficticio para la demostración
        foreach (var product in products)
        {
            product.ListPrice = product.ListPrice * 0.9m;
        }

        return products;
    }
}
