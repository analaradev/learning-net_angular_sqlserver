using Backend.Dtos;

namespace Backend.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDetailDto?> GetByIdAsync(int id);
    Task<List<ProductDto>> SearchByNameAsync(string name);
    Task<List<ProductNoteDto>?> GetNotesByProductIdAsync(int productId);
    Task<(ProductWriteResult Result, ProductNoteDto? ProductNote)> CreateNoteAsync(
        int productId,
        CreateProductNoteDto productNoteDto);
    Task<(ProductWriteResult Result, ProductDetailDto? Product)> CreateAsync(CreateProductDto productDto);
    Task<ProductWriteResult> UpdateAsync(int id, UpdateProductDto productDto);
    Task<(ProductWriteResult Result, ProductDetailDto? Product)> PatchAsync(int id, PatchProductDto productDto);
    Task<ProductWriteResult> DeleteAsync(int id);

    Task<ProductWithNotesDto?> GetByIdWithNotesAsync(int id);

}
