namespace Backend.Clean.Application.DTOs;

public class ProductWithNotesDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public string ProductNumber { get; set; } = "";
    public List<ProductNoteDto> Notes { get; set; } = [];
}
