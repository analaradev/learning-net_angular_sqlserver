namespace Backend.Dtos;

public class ProductDetailDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public string ProductNumber { get; set; } = "";
    public string? Color { get; set; }
    public decimal? ListPrice { get; set; }
    public decimal? Weight { get; set; }
}
