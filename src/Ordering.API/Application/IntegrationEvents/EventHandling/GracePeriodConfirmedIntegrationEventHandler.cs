using EventBus;
using MassTransit;
using MediatR;
using Ordering.API.Application.Commands;
using Shop.Contracts.OrderProcessor;

namespace Ordering.API.Application.IntegrationEvents.EventHandling;

public class GracePeriodConfirmedIntegrationEventHandler(
    IMediator mediator,
    ILogger<GracePeriodConfirmedIntegrationEventHandler> logger)
    : IConsumer<GracePeriodConfirmedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<GracePeriodConfirmedIntegrationEvent> context)
    {
        var message = context.Message;
        
        logger.LogInformation(
            "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            message.Id,
            message);

        var command = new SetAwaitingValidationOrderStatusCommand(message.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}