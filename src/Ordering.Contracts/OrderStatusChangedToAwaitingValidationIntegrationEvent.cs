using Ordering.Domain.OrderAggregate;
using Shop.Contracts.Orders;

namespace Ordering.Contracts;


public record OrderStatusChangedToAwaitingValidationIntegrationEvent(
    int OrderId,
    OrderStatus OrderStatus,
    string BuyerName,
    string BuyerIdentityGuid,
    IEnumerable<OrderStockItem> OrderStockItems);