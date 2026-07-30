namespace TsaElectronics.Api.Models.ProductModels;

public class ProductVariantModel
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? Color { get; set; }
    public string? Storage { get; set; }
    public string? Ram { get; set; }
}
