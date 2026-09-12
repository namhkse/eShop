using eShop.Catalog.API.IntegrationEvents.Events;
using MassTransit;
using MediatR;
using Ordering.API.Application.Orders.SetStockRejectedOrderStatus;

namespace Ordering.API.Application.IntegrationEvents.EventHandling;

public class OrderStockRejectedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStockRejectedIntegrationEventHandler> logger)
    : IConsumer<OrderStockRejectedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStockRejectedIntegrationEvent> context)
    {
        var message = context.Message;
        
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            message);
        
        var orderStockRejectedItems = message.OrderStockItems
            .FindAll(c => !c.HasStock)
            .Select(c => c.ProductId)
            .ToList();

        var command = new SetStockRejectedOrderStatusCommand(message.OrderId,
            orderStockRejectedItems);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}