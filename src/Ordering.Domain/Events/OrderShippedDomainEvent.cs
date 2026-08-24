using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Domain.Events;

public record OrderShippedDomainEvent(Order Order) : INotification;