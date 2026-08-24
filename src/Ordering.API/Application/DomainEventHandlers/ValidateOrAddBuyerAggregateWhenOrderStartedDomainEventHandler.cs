using MassTransit;
using MediatR;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.Events;
using Shop.Contracts.Orders;

namespace Ordering.API.Application.DomainEventHandlers;

public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(
    ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> logger,
    IBuyerRepository buyerRepository,
    IPublishEndpoint publishEndpoint)
    : INotificationHandler<OrderStartedDomainEvent>
{
    public async Task Handle(OrderStartedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var cardTypeId = domainEvent.CardTypeId != 0
            ? domainEvent.CardTypeId
            : 1;
        var buyer = await buyerRepository.FindAsync(domainEvent.UserId);
        
        var buyerExisted = buyer is not null;

        if (!buyerExisted)
        {
            buyer = new Buyer(domainEvent.UserId, domainEvent.UserName);
        }

        // REVIEW: The event this creates needs to be sent after SaveChanges has propagated the buyer Id. It currently only
        // works by coincidence. If we remove HiLo or if anything decides to yield earlier, it will break.

        buyer.VerifyOrAddPaymentMethod(cardTypeId,
            $"Payment Method on {DateTime.UtcNow}",
            domainEvent.CardNumber,
            domainEvent.CardSecurityNumber,
            domainEvent.CardHolderName,
            domainEvent.CardExpiration,
            domainEvent.Order.Id);

        if (!buyerExisted)
        {
            buyerRepository.Add(buyer);
        }

        await buyerRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);

        var integrationEvent = new OrderStatusChangedToSubmittedIntegrationEvent(domainEvent.Order.Id,
            domainEvent.Order.OrderStatus,
            buyer.Name,
            buyer.IdentityGuid);
        
        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}