namespace Backend.Models;

public class ProductoDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public string ProductNumber { get; set; } = "";
    public decimal? ListPrice { get; set; }
}