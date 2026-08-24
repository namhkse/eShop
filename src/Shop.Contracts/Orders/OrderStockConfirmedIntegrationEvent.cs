namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderStockConfirmedIntegrationEvent
{
    public int OrderId { get; }

    public OrderStockConfirmedIntegrationEvent(int orderId) => OrderId = orderId;
}
