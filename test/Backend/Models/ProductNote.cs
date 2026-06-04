using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("ProductNotes")]
public class ProductNote
{
    [Key]
    public int ProductNoteId { get; set; }

    [Column("ProductID")]
    public int ProductId { get; set; }

    [MaxLength(200)]
    public string Note { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public Product? Product { get; set; }
}
