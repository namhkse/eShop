
using Ordering.Domain.OrderAggregate;

namespace Ordering.Contracts;

public record OrderStatusChangedToCancelledIntegrationEvent(
    int OrderId,
    OrderStatus OrderStatus,
    string BuyerName,
    string BuyerIdentityGuid);

