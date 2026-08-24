using MassTransit;
using MediatR;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.Events;
using Ordering.Domain.OrderAggregate;
using Shop.Contracts.Orders;

namespace Ordering.API.Application.DomainEventHandlers;

public class OrderStatusChangedToPaidDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<OrderStatusChangedToPaidDomainEventHandler> logger,
    IBuyerRepository buyerRepository,
    IPublishEndpoint publishEndpoint)
    : INotificationHandler<OrderStatusChangedToPaidDomainEvent>
{
    public async Task Handle(OrderStatusChangedToPaidDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(domainEvent.OrderId);
        var buyer = await buyerRepository.FindByIdAsync(order.BuyerId.Value);

        var orderStockList = domainEvent.OrderItems
            .Select(orderItem => new OrderStockItem(orderItem.ProductId,
                orderItem.Units));

        var integrationEvent = new OrderStatusChangedToPaidIntegrationEvent(
            domainEvent.OrderId,
            order.OrderStatus,
            buyer.Name,
            buyer.IdentityGuid,
            orderStockList);

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}