using MassTransit;
using MediatR;
using Ordering.API.Application.Orders.CancelOrder;
using PaymentProcessor.Contracts;

namespace Ordering.API.Application.Consumers;

public class OrderPaymentFailedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderPaymentFailedIntegrationEventHandler> logger)
    : IConsumer<OrderPaymentFailedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderPaymentFailedIntegrationEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            message);

        var command = new CancelOrderCommand(message.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}