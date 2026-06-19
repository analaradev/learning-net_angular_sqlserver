using System.ComponentModel.DataAnnotations;

namespace Backend.Clean.Application.DTOs;

public class CreateProductNoteDto
{
    [Required]
    [MaxLength(200)]
    public string Note { get; set; } = "";
}
