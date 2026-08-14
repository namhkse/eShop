using MediatR;
using Ordering.Domain.BuyerAggregate;

namespace Ordering.Domain.Events;

public class BuyerAndPaymentMethodVerifiedDomainEvent(
    Buyer buyer,
    PaymentMethod paymentMethod,
    int orderId)
    : INotification
{
    public Buyer Buyer { get; private set; } = buyer;

    public PaymentMethod PaymentMethod { get; private set; } = paymentMethod;

    public int OrderId { get; private set; } = orderId;
}