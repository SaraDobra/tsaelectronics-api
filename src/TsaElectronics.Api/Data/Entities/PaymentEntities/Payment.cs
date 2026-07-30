using TsaElectronics.Api.Data.Entities.OrderEntities;

namespace TsaElectronics.Api.Data.Entities.PaymentEntities;

public class Payment : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string StripePaymentIntentId { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public decimal Amount { get; set; }
}
