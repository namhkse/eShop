using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.API.Application.Orders.SetPaidOrderStatus;

public class SetPaidOrderStatusCommandHandler(IOrderRepository orderRepository)
    : IRequestHandler<SetPaidOrderStatusCommand, bool>
{
    public async Task<bool> Handle(SetPaidOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // Simulate a work time for validating the payment
        await Task.Delay(10000, cancellationToken);

        var orderToUpdate = await orderRepository.GetAsync(command.OrderNumber);
        
        if (orderToUpdate == null)
        {
            return false;
        }

        orderToUpdate.SetPaidStatus();
        return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}