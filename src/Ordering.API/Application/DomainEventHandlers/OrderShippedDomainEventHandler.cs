using MassTransit;
using MediatR;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.Events;
using Ordering.Domain.OrderAggregate;
using Shop.Contracts.Orders;

namespace Ordering.API.Application.DomainEventHandlers;

public class OrderShippedDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<OrderShippedDomainEventHandler> logger,
    IBuyerRepository buyerRepository,
    IPublishEndpoint publishEndpoint)
    : INotificationHandler<OrderShippedDomainEvent>
{
    public async Task Handle(OrderShippedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(domainEvent.Order.Id);
        var buyer = await buyerRepository.FindByIdAsync(order.BuyerId.Value);

        var integrationEvent = new OrderStatusChangedToShippedIntegrationEvent(order.Id,
            order.OrderStatus,
            buyer.Name,
            buyer.IdentityGuid);
        
        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}