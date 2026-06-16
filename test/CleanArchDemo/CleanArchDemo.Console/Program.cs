using CleanArchDemo.Application;
using CleanArchDemo.Infrastructure;

namespace CleanArchDemo.ConsoleApp;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Instanciamos el repositorio concreto (Capa de Infraestructura)
        IProductRepository repository = new InMemoryProductRepository();

        // 2. Inyectamos el repositorio en el servicio (Capa de Aplicación / Core)
        var productService = new ProductService(repository);

        // 3. Llamamos a la lógica de negocio (Casos de Uso)
        var products = await productService.GetDiscountedProductsAsync();

        // 4. Mostramos el resultado en pantalla (Capa de Presentación)
        Console.WriteLine("=== PRODUCTOS DISPONIBLES EN CLEAN ARCHITECTURE ===");
        foreach (var product in products)
        {
            Console.WriteLine($"[ID: {product.Id}] - {product.Name} | Precio final (Con -10% desc): ${product.ListPrice}");
        }
    }
}
