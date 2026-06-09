using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        await Task.Delay(10000, cancellationToken);
        var products = await _productService.GetAllAsync();

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

    [HttpGet("busqueda-avanzada")]
    public async Task<IActionResult> AdvancedSearch(
        [FromQuery] string? name,
        [FromQuery] string? color,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        var products = await _productService.AdvancedSearchAsync(
            name,
            color,
            minPrice,
            maxPrice);

        return Ok(products);
    }

    [HttpGet("agrupados-por-color")]
    public async Task<IActionResult> GetProductsGroupedByColor()
    {
        var products = await _productService.GetProductsGroupedByColorAsync();
        return Ok(products);
    }

    [HttpGet("{id:int}/tiene-notas")]
    public async Task<IActionResult> ProductHasNotes(int id)
    {
        var result = await _productService.ProductHasNotesAsync(id);
        return Ok(result);
    }

    [HttpGet("notas-validas")]
    public async Task<IActionResult> AllNotesHaveText()
    {
        var result = await _productService.AllNotesHaveTextAsync();
        return Ok(result);
    }

    [HttpGet("por-numero/{productNumber}")]
    public async Task<IActionResult> GetByProductNumber(string productNumber)
    {
        var product = await _productService.GetByProductNumberAsync(productNumber);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("tracking-comparacion")]
    public async Task<IActionResult> GetTrackingComparison()
    {
        var result = await _productService.GetTrackingComparisonAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}/notas")]
    public async Task<IActionResult> GetNotes(int id)
    {
        var productNotes = await _productService.GetNotesByProductIdAsync(id);

        if (productNotes is null)
        {
            return NotFound();
        }

        return Ok(productNotes);
    }

    [HttpPost("{id:int}/notas")]
    public async Task<IActionResult> CreateNote(int id, [FromBody] CreateProductNoteDto productNoteDto)
    {
        if (id <= 0)
        {
            return BadRequest("El id debe ser mayor que cero.");
        }

        var (result, productNote) = await _productService.CreateNoteAsync(id, productNoteDto);

        if (result == ProductWriteResult.NotFound)
        {
            return NotFound();
        }

        return CreatedAtAction(
            nameof(GetNotes),
            new { id },
            productNote);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto productDto)
    {
        var (result, createdProduct) = await _productService.CreateAsync(productDto);

        if (result == ProductWriteResult.Conflict)
        {
            return Conflict($"Ya existe un producto con el numero '{productDto.ProductNumber}'.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProduct!.ProductId },
            createdProduct);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto productDto)
    {
        if (id <= 0)
        {
            return BadRequest("El id debe ser mayor que cero.");
        }

        var result = await _productService.UpdateAsync(id, productDto);

        if (result == ProductWriteResult.NotFound)
        {
            return NotFound();
        }

        if (result == ProductWriteResult.Conflict)
        {
            return Conflict($"Ya existe un producto con el numero '{productDto.ProductNumber}'.");
        }

        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] PatchProductDto productDto)
    {
        if (id <= 0)
        {
            return BadRequest("El id debe ser mayor que cero.");
        }

        var (result, updatedProduct) = await _productService.PatchAsync(id, productDto);

        if (result == ProductWriteResult.NotFound)
        {
            return NotFound();
        }

        if (result == ProductWriteResult.Conflict)
        {
            return Conflict($"Ya existe un producto con el numero '{productDto.ProductNumber}'.");
        }

        return Ok(updatedProduct);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest("El id debe ser mayor que cero.");
        }

        var result = await _productService.DeleteAsync(id);

        if (result == ProductWriteResult.NotFound)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("{id:int}/con-notas")]
    public async Task<IActionResult> GetByIdWithNotes(int id)
    {
        var product = await _productService.GetByIdWithNotesAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("raw-sql/precio-minimo")]
    public async Task<IActionResult> GetProductsByMinPriceWithRawSql([FromQuery] decimal minPrice)
    {
        var products = await _productService.GetProductsByMinPriceWithRawSqlAsync(minPrice);
        return Ok(products);
    }

    [HttpPost("transaccion/con-nota")]
    public async Task<IActionResult> CreateProductWithNoteInTransaction(
        [FromBody] CreateProductWithNoteDto productDto)
    {
        var (result, product) = await _productService.CreateProductWithNoteInTransactionAsync(productDto);

        if (result == ProductWriteResult.Conflict)
        {
            return Conflict($"Ya existe un producto con el numero '{productDto.Product.ProductNumber}'.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = product!.ProductId },
            product);
    }

    [HttpGet("prueba-cancelacion/con-token")]
    public async Task<IActionResult> TestCancellationToken(CancellationToken cancellationToken)
    {
        Console.WriteLine("con token: inicio la peticion");

        try
        {
            await Task.Delay(10000, cancellationToken);

            Console.WriteLine("con token: termino el delay y va a consultar productos");

            var products = await _productService.GetAllAsync(cancellationToken);

            Console.WriteLine("con token: termino la consulta de productos");

            return Ok(products);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("con token: la peticion fue cancelada");
            return new EmptyResult();
        }
    }

    [HttpGet("prueba-cancelacion/sin-token")]
    public async Task<IActionResult> TestWithoutCancellationToken()
    {
        Console.WriteLine("sin token: inicio la peticion");

        await Task.Delay(10000);

        Console.WriteLine("sin token: termino el delay aunque el cliente haya cancelado y va a consultar productos");

        var products = await _productService.GetAllAsync(CancellationToken.None);

        Console.WriteLine("sin token: termino la consulta de productos aunque el cliente haya cancelado");

        return Ok(products);
    }

}
