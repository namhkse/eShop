using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.API.Application.Orders.SetStockConfirmedOrderStatus;

public class SetStockConfirmedOrderStatusCommandHandler(IOrderRepository orderRepository)
    : IRequestHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    public async Task<bool> Handle(SetStockConfirmedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // Simulate a work time for confirming the stock
        await Task.Delay(10000, cancellationToken);

        var orderToUpdate = await orderRepository.GetAsync(command.OrderNumber);
        if (orderToUpdate == null)
        {
            return false;
        }

        orderToUpdate.SetStockConfirmedStatus();
        return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}