using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shop.Contracts.Orders;

namespace Shop.WebApp.Services.OrderStatus.IntegrationEvents.EventHandling;

public class OrderStatusChangedToCancelledIntegrationEventHandler(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToCancelledIntegrationEventHandler> logger)
    : IConsumer<OrderStatusChangedToCancelledIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToCancelledIntegrationEvent> context)
    {
        var message = context.Message;
        
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            message);
        
        await orderStatusNotificationService.NotifyOrderStatusChangedAsync(message.BuyerIdentityGuid);
    }
}