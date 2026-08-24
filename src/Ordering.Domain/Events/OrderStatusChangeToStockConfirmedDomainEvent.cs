using MediatR;

namespace Ordering.Domain.Events;

public record OrderStatusChangeToStockConfirmedDomainEvent(int OrderId) : INotification;