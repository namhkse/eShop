using MediatR;
using Ordering.API.Application.Commands;
using Ordering.Infrastructure.Idempotency;

namespace Ordering.API.Application.Orders.SetShipOrder;

public class ShipOrderIdentifiedCommandHandler : IdentifiedCommandHandler<ShipOrderCommand, bool>
{
    public ShipOrderIdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<ShipOrderCommand, bool>> logger)
        : base(mediator,
            requestManager,
            logger)
    {
    }

    protected override bool CreateResultForDuplicateRequest()
    {
        return true;
    }
}