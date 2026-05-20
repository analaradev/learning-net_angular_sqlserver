namespace Backend.Models;

public class ProductoDetalleDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public string ProductNumber { get; set; } = "";
    public string? Color { get; set; }
    public decimal? ListPrice { get; set; }
    public decimal? Weight { get; set; }
}
