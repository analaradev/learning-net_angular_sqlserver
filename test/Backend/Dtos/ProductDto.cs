namespace Backend.Dtos;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public string ProductNumber { get; set; } = "";
    public decimal? ListPrice { get; set; }
}
