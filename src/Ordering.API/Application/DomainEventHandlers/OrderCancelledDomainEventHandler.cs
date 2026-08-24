using MassTransit;
using MediatR;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.Events;
using Ordering.Domain.OrderAggregate;
using Shop.Contracts.Orders;

namespace Ordering.API.Application.DomainEventHandlers;

public class OrderCancelledDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<OrderCancelledDomainEventHandler> logger,
    IBuyerRepository buyerRepository,
    IPublishEndpoint publishEndpoint)
    : INotificationHandler<OrderCancelledDomainEvent>
{
    public async Task Handle(OrderCancelledDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        // TODO: enhance with trace
        // OrderingApiTrace.LogOrderStatusUpdated(_logger,
        //     domainEvent.Order.Id,
        //     OrderStatus.Cancelled);

        var order = await orderRepository.GetAsync(domainEvent.Order.Id);
        var buyer = await buyerRepository.FindByIdAsync(order.BuyerId!.Value);

        var evt = new OrderStatusChangedToCancelledIntegrationEvent(order.Id,
            order.OrderStatus,
            buyer.Name,
            buyer.IdentityGuid);
        
        await publishEndpoint.Publish(evt, cancellationToken);
    }
}