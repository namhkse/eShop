namespace Ordering.Contracts;

public record OrderStockConfirmedIntegrationEvent
{
    public int OrderId { get; }

    public OrderStockConfirmedIntegrationEvent(int orderId) => OrderId = orderId;
}
