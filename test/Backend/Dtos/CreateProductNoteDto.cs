using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

public class CreateProductNoteDto
{
    [Required]
    [MaxLength(200)]
    public string Note { get; set; } = "";
}
