using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Domain.Events;

public class OrderShippedDomainEvent(Order order) : INotification
{
    public Order Order => order;
}