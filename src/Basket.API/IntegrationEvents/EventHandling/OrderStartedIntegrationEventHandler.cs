using Basket.API.Repositories;
using MassTransit;
using Shop.Contracts.Orders;

namespace Basket.API.IntegrationEvents.EventHandling;

public class OrderStartedIntegrationEventHandler(
    IBasketRepository repository,
    ILogger<OrderStartedIntegrationEventHandler> logger)
    : IConsumer<OrderStartedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStartedIntegrationEvent> context)
    {
        logger.LogInformation(
            "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            context.Message);

        await repository.DeleteBasketAsync(context.Message.UserId);
    }
}