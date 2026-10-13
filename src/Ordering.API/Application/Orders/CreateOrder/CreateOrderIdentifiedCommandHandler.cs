using MediatR;
using Ordering.API.Application.Commands;
using Ordering.Infrastructure.Idempotency;

namespace Ordering.API.Application.Orders.CreateOrder;

public class CreateOrderIdentifiedCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>> logger)
    : IdentifiedCommandHandler<CreateOrderCommand, bool>(mediator,
        requestManager,
        logger)
{
    protected override bool CreateResultForDuplicateRequest()
    {
        return true;
    }
}