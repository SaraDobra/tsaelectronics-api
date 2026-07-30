namespace TsaElectronics.Api.Data.Entities.ProductEntities;

public class ProductVariant : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    // Distinguishing attributes, e.g. Color: "Space Black", Storage: "256GB", Ram: "16GB"
    public string? Color { get; set; }
    public string? Storage { get; set; }
    public string? Ram { get; set; }
}
