using MediatR;

namespace Ordering.Domain.Events;

public class OrderStatusChangeToStockConfirmedDomainEvent
    : INotification
{
    public int OrderId { get; }

    public OrderStatusChangeToStockConfirmedDomainEvent(int orderId)
    {
        OrderId = orderId; 
    }
}