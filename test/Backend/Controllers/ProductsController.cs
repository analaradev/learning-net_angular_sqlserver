using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        // Ok devuelve un 200 con el resultado.

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product is null)
        {
            // NotFound devuelve un 404.
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> SearchByName([FromQuery] string nombre)
    {
        var products = await _productService.SearchByNameAsync(nombre);
        return Ok(products);
    }
}
