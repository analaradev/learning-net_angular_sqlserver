namespace Backend.Dtos;

public class ProductNoteDto
{
    public int ProductNoteId { get; set; }
    public int ProductId { get; set; }
    public string Note { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
