namespace Backend.Clean.Domain.Entities;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public string ProductNumber { get; set; } = "";
    public string? Color { get; set; }
    public short SafetyStockLevel { get; set; }
    public short ReorderPoint { get; set; }
    public decimal StandardCost { get; set; }
    public decimal ListPrice { get; set; }
    public string? Size { get; set; }
    public decimal? Weight { get; set; }
    public int DaysToManufacture { get; set; }
    public DateTime SellStartDate { get; set; }
    public DateTime? SellEndDate { get; set; }
    public DateTime? DiscontinuedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? ProductSubcategoryId { get; set; }

    public List<ProductNote> ProductNotes { get; set; } = [];
}
