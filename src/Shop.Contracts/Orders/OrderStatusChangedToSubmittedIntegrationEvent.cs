using Ordering.Domain.OrderAggregate;

namespace Shop.Contracts.Orders;

public record OrderStatusChangedToSubmittedIntegrationEvent(
    int OrderId,
    OrderStatus OrderStatus,
    string BuyerName,
    string BuyerIdentityGuid);
