namespace Backend.Clean.Domain.Entities;

public class ProductNote
{
    public int ProductNoteId { get; set; }
    public int ProductId { get; set; }
    public string Note { get; set; } = "";
    public DateTime CreatedAt { get; set; }

    public Product? Product { get; set; }
}
