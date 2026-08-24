using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shop.Contracts.Orders;

namespace Shop.WebApp.Services.OrderStatus.IntegrationEvents.EventHandling;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
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