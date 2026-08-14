using System.ComponentModel.DataAnnotations;
using Ordering.Domain.Events;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.BuyerAggregate;

public class Buyer : Entity, IAggregateRoot
{
    [Required] public string IdentityGuid { get; private set; }

    public string Name { get; private set; }

    private List<PaymentMethod> _paymentMethods;

    public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

    protected Buyer()
    {
        _paymentMethods = new List<PaymentMethod>();
    }

    public Buyer(string identityGuid, string name) : this()
    {
        IdentityGuid = !string.IsNullOrEmpty(identityGuid)
            ? identityGuid
            : throw new ArgumentNullException(nameof(identityGuid));

        Name = !string.IsNullOrEmpty(name)
            ? name
            : throw new ArgumentNullException(nameof(name));
    }

    public PaymentMethod VerifyOrAddPaymentMethod(
        int cardTypeId,
        string alias,
        string cardNumber,
        string securityNumber,
        string cardHolderName,
        DateTime expiration,
        int orderId)
    {
        var existingPayment = _paymentMethods
            .SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

        if (existingPayment is not null)
        {
            AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(
                this,
                existingPayment,
                orderId));

            return existingPayment;
        }

        var payment = new PaymentMethod(
            cardTypeId,
            alias,
            cardNumber,
            securityNumber,
            cardHolderName,
            expiration);

        _paymentMethods.Add(payment);

        AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(
            this,
            payment,
            orderId));

        return payment;
    }
}