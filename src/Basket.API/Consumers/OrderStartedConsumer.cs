using Basket.API.Repositories;
using MassTransit;
using Ordering.Contracts;

namespace Basket.API.Consumers;

public class OrderStartedConsumer(
    IBasketRepository repository,
    ILogger<OrderStartedConsumer> logger)
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