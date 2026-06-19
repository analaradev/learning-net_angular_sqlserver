namespace Backend.Clean.Application.DTOs;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public string ProductNumber { get; set; } = "";
    public decimal? ListPrice { get; set; }
}
