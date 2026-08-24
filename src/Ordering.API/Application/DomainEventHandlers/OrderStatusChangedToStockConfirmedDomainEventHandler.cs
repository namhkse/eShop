using MassTransit;
using MediatR;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.Events;
using Ordering.Domain.OrderAggregate;
using Shop.Contracts.Orders;

namespace Ordering.API.Application.DomainEventHandlers;

public class OrderStatusChangedToStockConfirmedDomainEventHandler(
    IOrderRepository orderRepository,
    IBuyerRepository buyerRepository,
    ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> logger,
    IPublishEndpoint publishEndpoint)
    : INotificationHandler<OrderStatusChangeToStockConfirmedDomainEvent>
{
    public async Task Handle(OrderStatusChangeToStockConfirmedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(domainEvent.OrderId);
        var buyer = await buyerRepository.FindByIdAsync(order.BuyerId.Value);

        await publishEndpoint.Publish(new OrderStatusChangedToStockConfirmedIntegrationEvent(order.Id,
            order.OrderStatus,
            buyer.Name,
            buyer.IdentityGuid), cancellationToken);
    }
}