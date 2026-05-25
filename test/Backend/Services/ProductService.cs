using Backend.Data;
using Backend.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class ProductService
{
    private readonly AdventureWorksContext _context;
    // AutoMapper:
    // private readonly IMapper _mapper;

    public ProductService(AdventureWorksContext context)
    {
        _context = context;
        // AutoMapper:
        // _mapper = mapper;
    }

//task es para  que sea asincrono el metodo 
    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _context.Products
            .AsNoTracking() // este es solo se usa para lectura
            .OrderBy(product => product.Name)
            .Take(10)
            .ToListAsync();

     //lynq projection
        // return await _context.Products
        //     .AsNoTracking()
        //     .OrderBy(product => product.Name)
        //     .Take(10)
        //     .Select(product => new ProductDto
        //     {
        //         ProductId = product.ProductId,
        //         Name = product.Name,
        //         ProductNumber = product.ProductNumber,
        //         ListPrice = product.ListPrice
        //     })
        //     .ToListAsync();

        // automaper

        // return _mapper.Map<List<ProductDto>>(products);

        return products.Adapt<List<ProductDto>>();
    }

    public async Task<ProductDetailDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Where(product => product.ProductId == id)
            .FirstOrDefaultAsync();

        // return await _context.Products
        //     .AsNoTracking()
        //     .Where(product => product.ProductId == id)
        //     .Select(product => new ProductDetailDto
        //     {
        //         ProductId = product.ProductId,
        //         Name = product.Name,
        //         ProductNumber = product.ProductNumber,
        //         Color = product.Color,
        //         ListPrice = product.ListPrice,
        //         Weight = product.Weight
        //     })
        //     .FirstOrDefaultAsync();
        // AutoMapper:
        // return _mapper.Map<ProductDetailDto?>(product);

        return product.Adapt<ProductDetailDto?>();
    }

    //task es para  que sea asincrono
    public async Task<List<ProductDto>> SearchByNameAsync(string name)
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(product => product.Name.Contains(name))
            .OrderBy(product => product.Name)
            .Take(20)
            .ToListAsync();

        // AutoMapper:
        // return _mapper.Map<List<ProductDto>>(products);

        return products.Adapt<List<ProductDto>>();
    }
}
