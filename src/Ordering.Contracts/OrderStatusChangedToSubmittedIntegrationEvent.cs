using Ordering.Domain.OrderAggregate;

namespace Ordering.Contracts;

public record OrderStatusChangedToSubmittedIntegrationEvent(
    int OrderId,
    OrderStatus OrderStatus,
    string BuyerName,
    string BuyerIdentityGuid);
