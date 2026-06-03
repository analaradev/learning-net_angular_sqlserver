using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

public class PatchProductDto
{
    [MaxLength(50)]
    public string? Name { get; set; }

    [MaxLength(25)]
    public string? ProductNumber { get; set; }

    [MaxLength(15)]
    public string? Color { get; set; }

    [Range(0, short.MaxValue)]
    public short? SafetyStockLevel { get; set; }

    [Range(0, short.MaxValue)]
    public short? ReorderPoint { get; set; }

    [Range(0, 999999.99)]
    public decimal? StandardCost { get; set; }

    [Range(0, 999999.99)]
    public decimal? ListPrice { get; set; }

    [MaxLength(5)]
    public string? Size { get; set; }

    [Range(0, 999999.99)]
    public decimal? Weight { get; set; }

    [Range(0, 365)]
    public int? DaysToManufacture { get; set; }
}
