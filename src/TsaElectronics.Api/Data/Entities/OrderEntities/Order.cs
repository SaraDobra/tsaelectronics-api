using TsaElectronics.Api.Data.Entities.IdentityEntities;

namespace TsaElectronics.Api.Data.Entities.OrderEntities;

public class Order : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }

    public ShippingAddress ShippingAddress { get; set; } = null!;

    public ICollection<OrderItem> Items { get; set; } = [];
}
