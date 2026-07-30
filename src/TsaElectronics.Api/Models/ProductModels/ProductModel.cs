namespace TsaElectronics.Api.Models.ProductModels;

public class ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<ProductVariantModel> Variants { get; set; } = [];
    public List<string> ImageUrls { get; set; } = [];
}
