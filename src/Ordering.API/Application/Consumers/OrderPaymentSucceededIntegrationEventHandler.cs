using MassTransit;
using MediatR;
using Ordering.API.Application.Orders.SetPaidOrderStatus;
using Ordering.Contracts;

namespace Ordering.API.Application.Consumers;

public class OrderPaymentSucceededIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderPaymentSucceededIntegrationEventHandler> logger)
    : IConsumer<OrderPaymentSucceededIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderPaymentSucceededIntegrationEvent> context)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            context.Message);

        var command = new SetPaidOrderStatusCommand(context.Message.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}