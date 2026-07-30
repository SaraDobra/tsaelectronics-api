namespace TsaElectronics.Api.Data.Entities.OrderEntities;

public enum OrderStatus
{
    Pending,
    PaymentReceived,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Refunded
}
