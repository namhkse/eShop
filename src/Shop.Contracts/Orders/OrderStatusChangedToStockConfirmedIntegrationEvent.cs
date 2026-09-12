namespace Shop.Contracts.Orders;

public record OrderStatusChangedToStockConfirmedIntegrationEvent(
    int OrderId,
    OrderStatus OrderStatus,
    string BuyerName,
    string BuyerIdentityGuid);
