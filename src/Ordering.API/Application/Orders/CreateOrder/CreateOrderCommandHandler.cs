using MassTransit;
using MediatR;
using Ordering.API.Infrastructure.Services;
using Ordering.Domain.OrderAggregate;
using Shop.Contracts.Orders;

namespace Ordering.API.Application.Orders.CreateOrder;

public class CreateOrderCommandHandler(
    IMediator mediator,
    IPublishEndpoint publishEndpoint,
    IOrderRepository orderRepository,
    IIdentityService identityService,
    ILogger<CreateOrderCommandHandler> logger)
    : IRequestHandler<CreateOrderCommand, bool>
{
    public async Task<bool> Handle(CreateOrderCommand message,
        CancellationToken cancellationToken)
    {
        // Add Integration event to clean the basket
        await publishEndpoint.Publish(new OrderStartedIntegrationEvent(message.UserId),
            cancellationToken);

        var address = new Address(message.Street,
            message.City,
            message.State,
            message.Country,
            message.ZipCode);

        var order = new Order(message.UserId,
            message.UserName,
            address,
            message.CardTypeId,
            message.CardNumber,
            message.CardSecurityNumber,
            message.CardHolderName,
            message.CardExpiration);

        foreach (var item in message.OrderItems)
        {
            order.AddOrderItem(item.ProductId,
                item.ProductName,
                item.UnitPrice,
                item.Discount,
                item.PictureUrl,
                item.Units);
        }

        logger.LogInformation("Creating Order - Order: {@Order}",
            order);

        orderRepository.Add(order);

        return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}