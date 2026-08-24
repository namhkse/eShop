using eShop.Ordering.API.Application.IntegrationEvents.Events;
using EventBus;
using MassTransit;
using MediatR;
using Ordering.API.Application.Orders.SetStockConfirmedOrderStatus;

namespace Ordering.API.Application.IntegrationEvents.EventHandling;

public class OrderStockConfirmedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStockConfirmedIntegrationEventHandler> logger) :
    IConsumer<OrderStockConfirmedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStockConfirmedIntegrationEvent> context)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            context.Message);

        var command = new SetStockConfirmedOrderStatusCommand(context.Message.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }

}