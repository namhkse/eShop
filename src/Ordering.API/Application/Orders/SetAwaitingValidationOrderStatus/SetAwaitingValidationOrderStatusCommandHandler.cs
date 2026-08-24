using MediatR;
using Ordering.API.Application.Commands;
using Ordering.Domain.OrderAggregate;

namespace Ordering.API.Application.Orders.SetAwaitingValidationOrderStatus;

public class SetAwaitingValidationOrderStatusCommandHandler(
    IOrderRepository orderRepository)
    : IRequestHandler<SetAwaitingValidationOrderStatusCommand, bool>
{

    public async Task<bool> Handle(
        SetAwaitingValidationOrderStatusCommand command,
        CancellationToken cancellationToken)
    {
        var orderToUpdate = await orderRepository.GetAsync(command.OrderNumber);

        if (orderToUpdate == null)
            return false;

        orderToUpdate.SetAwaitingValidationStatus();

        return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}