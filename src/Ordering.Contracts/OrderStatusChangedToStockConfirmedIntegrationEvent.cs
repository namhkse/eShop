using Ordering.Domain.OrderAggregate;

namespace Ordering.Contracts;

public record OrderStatusChangedToStockConfirmedIntegrationEvent(
    int OrderId,
    OrderStatus OrderStatus,
    string BuyerName,
    string BuyerIdentityGuid);
