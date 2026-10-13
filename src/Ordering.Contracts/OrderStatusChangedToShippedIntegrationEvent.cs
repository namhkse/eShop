using Ordering.Domain.OrderAggregate;

namespace Ordering.Contracts;

public record OrderStatusChangedToShippedIntegrationEvent(
    int OrderId,
    OrderStatus OrderStatus,
    string BuyerName,
    string BuyerIdentityGuid);

