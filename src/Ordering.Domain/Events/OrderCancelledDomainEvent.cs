using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Domain.Events;

public record OrderCancelledDomainEvent(Order Order) : INotification;