using MassTransit;
using Microsoft.Extensions.Options;
using Shop.Contracts.Orders;
using Shop.Contracts.Payments;
using OrderPaymentSucceededIntegrationEvent = Shop.Contracts.Payments.OrderPaymentSucceededIntegrationEvent;

namespace PaymentProcessor.IntegrationEvents.EventHandling;

public class OrderStatusChangedToStockConfirmedIntegrationEventHandler(
    IPublishEndpoint eventBus,
    IOptionsMonitor<PaymentOptions> options,
    ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger)
    : IConsumer<OrderStatusChangedToStockConfirmedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToStockConfirmedIntegrationEvent> context)
    {
        var message = context.Message;

        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            context.Message.GetType().Name);

        // Instead of a real payment we just take the env. var to simulate the payment can be successful, or it can fail

        if (options.CurrentValue.PaymentSucceeded)
        {
            var messageId = NewId.NextGuid();
            var successMessage = new OrderPaymentSucceededIntegrationEvent(message.OrderId);

            logger.LogInformation(
                "Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})",
                messageId,
                successMessage);

            await eventBus.Publish(
                successMessage,
                ctx => ctx.MessageId = messageId);
        }
        else
        {
            var messageId = NewId.NextGuid();
            var failedMessage = new OrderPaymentFailedIntegrationEvent(message.OrderId);

            logger.LogInformation(
                "Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})",
                messageId,
                failedMessage);

            await eventBus.Publish(
                failedMessage,
                ctx => ctx.MessageId = messageId);
        }
    }
}