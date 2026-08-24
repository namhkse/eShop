using MediatR;
using Ordering.Domain.BuyerAggregate;

namespace Ordering.Domain.Events;

public record BuyerAndPaymentMethodVerifiedDomainEvent(
    Buyer Buyer,
    PaymentMethod Payment,
    int OrderId)
    : INotification;