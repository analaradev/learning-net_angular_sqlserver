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
}
