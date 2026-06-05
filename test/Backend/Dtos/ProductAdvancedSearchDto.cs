namespace Backend.Dtos;

public class ProductAdvancedSearchDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public string ProductNumber { get; set; } = "";
    public string? Color { get; set; }
    public decimal ListPrice { get; set; }
    public int NotesCount { get; set; }
}
