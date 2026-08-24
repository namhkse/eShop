namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

public record OrderPaymentFailedIntegrationEvent 
{
    public int OrderId { get; }

    public OrderPaymentFailedIntegrationEvent(int orderId) => OrderId = orderId;
}
