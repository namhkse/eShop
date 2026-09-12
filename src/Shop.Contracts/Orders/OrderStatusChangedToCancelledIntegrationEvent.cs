
namespace Shop.Contracts.Orders;

public record OrderStatusChangedToCancelledIntegrationEvent(
    int OrderId,
    OrderStatus OrderStatus,
    string BuyerName,
    string BuyerIdentityGuid);

