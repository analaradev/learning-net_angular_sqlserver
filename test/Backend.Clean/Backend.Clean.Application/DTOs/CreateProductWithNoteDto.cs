using System.ComponentModel.DataAnnotations;

namespace Backend.Clean.Application.DTOs;

public class CreateProductWithNoteDto
{
    [Required]
    public CreateProductDto Product { get; set; } = new();

    [Required]
    [MaxLength(200)]
    public string Note { get; set; } = "";
}
