using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Domain.Events;

public class OrderCancelledDomainEvent(Order order) : INotification
{
    public Order Order => order;
}