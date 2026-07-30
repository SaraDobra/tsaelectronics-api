using TsaElectronics.Api.Data.Entities.ProductEntities;

namespace TsaElectronics.Api.Data.Entities.OrderEntities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public Guid ProductVariantId { get; set; }
    public ProductVariant ProductVariant { get; set; } = null!;

    // Snapshotted at time of purchase so historical orders stay accurate even if
    // the product is later renamed, repriced, or deleted.
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
