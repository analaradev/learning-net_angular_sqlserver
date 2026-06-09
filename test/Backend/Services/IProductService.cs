using Backend.Dtos;

namespace Backend.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDetailDto?> GetByIdAsync(int id);
    Task<List<ProductDto>> SearchByNameAsync(string name);
    Task<List<ProductAdvancedSearchDto>> AdvancedSearchAsync(
        string? name,
        string? color,
        decimal? minPrice,
        decimal? maxPrice);
    Task<List<ProductColorGroupDto>> GetProductsGroupedByColorAsync();
    Task<List<ProductNoteDto>?> GetNotesByProductIdAsync(int productId);
    Task<ProductHasNotesDto> ProductHasNotesAsync(int productId);
    Task<NotesValidationDto> AllNotesHaveTextAsync();
    Task<ProductDto?> GetByProductNumberAsync(string productNumber);
    Task<TrackingComparisonDto> GetTrackingComparisonAsync();
    Task<(ProductWriteResult Result, ProductNoteDto? ProductNote)> CreateNoteAsync(
        int productId,
        CreateProductNoteDto productNoteDto);
    Task<(ProductWriteResult Result, ProductDetailDto? Product)> CreateAsync(CreateProductDto productDto);
    Task<ProductWriteResult> UpdateAsync(int id, UpdateProductDto productDto);
    Task<(ProductWriteResult Result, ProductDetailDto? Product)> PatchAsync(int id, PatchProductDto productDto);
    Task<ProductWriteResult> DeleteAsync(int id);
    Task<ProductWithNotesDto?> GetByIdWithNotesAsync(int id);
    Task<List<ProductDto>> GetProductsByMinPriceWithRawSqlAsync(decimal minPrice);
    Task<(ProductWriteResult Result, ProductDetailDto? Product)> CreateProductWithNoteInTransactionAsync(
        CreateProductWithNoteDto productDto);

    Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken);
}
