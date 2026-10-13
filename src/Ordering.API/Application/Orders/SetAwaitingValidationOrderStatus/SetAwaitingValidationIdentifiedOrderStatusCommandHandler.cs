using MediatR;
using Ordering.API.Application.Commands;
using Ordering.Infrastructure.Idempotency;

namespace Ordering.API.Application.Orders.SetAwaitingValidationOrderStatus;

public class SetAwaitingValidationIdentifiedOrderStatusCommandHandler : IdentifiedCommandHandler<SetAwaitingValidationOrderStatusCommand, bool>
{
    public SetAwaitingValidationIdentifiedOrderStatusCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<SetAwaitingValidationOrderStatusCommand, bool>> logger)
        : base(mediator, requestManager, logger)
    {
    }

    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for processing order.
    }
}