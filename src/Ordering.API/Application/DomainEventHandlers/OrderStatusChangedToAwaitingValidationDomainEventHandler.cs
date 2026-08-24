using MassTransit;
using MediatR;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.Events;
using Ordering.Domain.OrderAggregate;
using Shop.Contracts.Orders;
using OrderStockItem = Shop.Contracts.Orders.OrderStockItem;

namespace Ordering.API.Application.DomainEventHandlers;

public class OrderStatusChangedToAwaitingValidationDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler> logger,
    IBuyerRepository buyerRepository,
    IPublishEndpoint publishEndpoint)
    : INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
{

    public async Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(domainEvent.OrderId);
        var buyer = await buyerRepository.FindByIdAsync(order.BuyerId!.Value);

        var orderStockList = domainEvent.OrderItems
            .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units));

        var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(order.Id,
            order.OrderStatus,
            buyer.Name,
            buyer.IdentityGuid,
            orderStockList);
        
        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}