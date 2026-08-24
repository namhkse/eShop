using MediatR;
using Ordering.Domain.Events;
using Ordering.Domain.OrderAggregate;

namespace Ordering.API.Application.DomainEventHandlers;

public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler> logger)
    :
        INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
{
    public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var orderToUpdate = await orderRepository.GetAsync(domainEvent.OrderId);
        orderToUpdate.SetPaymentMethodVerified(domainEvent.Buyer.Id, domainEvent.Payment.Id);
    }
}