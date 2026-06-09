using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

public class CreateProductWithNoteDto
{
    [Required]
    public CreateProductDto Product { get; set; } = new();

    [Required]
    [MaxLength(200)]
    public string Note { get; set; } = "";
}