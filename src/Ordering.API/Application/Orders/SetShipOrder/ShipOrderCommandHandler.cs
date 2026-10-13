using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.API.Application.Orders.SetShipOrder;

public class ShipOrderCommandHandler(
    IOrderRepository orderRepository) : IRequestHandler<ShipOrderCommand, bool>
{
    public async Task<bool> Handle(ShipOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderNumber);

        if (order == null)
            return false;

        order.SetShippedStatus();

        return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}