using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("Product", Schema = "Production")]
public class Product
{
    [Key]
    [Column("ProductID")]
    public int ProductId { get; set; }

    [Column("Name")]
    [MaxLength(50)]
    public string Name { get; set; } = "";

    [Column("ProductNumber")]
    [MaxLength(25)]
    public string ProductNumber { get; set; } = "";

    [Column("Color")]
    [MaxLength(15)]
    public string? Color { get; set; }

    [Column("StandardCost", TypeName = "money")]
    public decimal StandardCost { get; set; }

    [Column("ListPrice", TypeName = "money")]
    public decimal ListPrice { get; set; }

    [Column("Size")]
    [MaxLength(5)]
    public string? Size { get; set; }

    [Column("Weight", TypeName = "decimal(8, 2)")]
    public decimal? Weight { get; set; }

    [Column("SellStartDate")]
    public DateTime SellStartDate { get; set; }

    [Column("SellEndDate")]
    public DateTime? SellEndDate { get; set; }

    [Column("DiscontinuedDate")]
    public DateTime? DiscontinuedDate { get; set; }

    [Column("ModifiedDate")]
    public DateTime ModifiedDate { get; set; }
}
