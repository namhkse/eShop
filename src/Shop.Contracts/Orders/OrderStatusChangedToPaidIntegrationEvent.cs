using Ordering.Domain.OrderAggregate;

namespace Shop.Contracts.Orders;


public record OrderStatusChangedToPaidIntegrationEvent(
    int OrderId,
    OrderStatus OrderStatus,
    string BuyerName,
    string BuyerIdentityGuid,
    IEnumerable<OrderStockItem> OrderStockItems);