using MediatR;
using Ordering.API.Application.Commands;
using Ordering.Infrastructure.Idempotency;

namespace Ordering.API.Application.Orders.SetStockConfirmedOrderStatus;

public class SetStockConfirmedOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    public SetStockConfirmedOrderStatusIdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand, bool>> logger)
        : base(mediator, requestManager, logger)
    {
    }

    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for processing order.
    }
}