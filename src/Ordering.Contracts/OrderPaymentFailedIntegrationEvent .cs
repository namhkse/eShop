namespace Ordering.Contracts;

public record OrderPaymentFailedIntegrationEvent 
{
    public int OrderId { get; }

    public OrderPaymentFailedIntegrationEvent(int orderId) => OrderId = orderId;
}
