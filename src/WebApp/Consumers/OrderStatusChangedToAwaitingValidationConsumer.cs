using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Contracts;
using Shop.WebApp.Services.OrderStatus;

namespace Shop.WebApp.Consumers;

public class OrderStatusChangedToAwaitingValidationConsumer(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToAwaitingValidationConsumer> logger)
    : IConsumer<OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToAwaitingValidationIntegrationEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            message);
        
        await orderStatusNotificationService.NotifyOrderStatusChangedAsync(message.BuyerIdentityGuid);
    }
}