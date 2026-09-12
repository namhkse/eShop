namespace Shop.Contracts.Orders;

public record OrderStatusChangedToShippedIntegrationEvent(
    int OrderId,
    OrderStatus OrderStatus,
    string BuyerName,
    string BuyerIdentityGuid);

