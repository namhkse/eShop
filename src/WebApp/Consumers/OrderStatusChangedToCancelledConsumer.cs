using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Contracts;
using Shop.WebApp.Services.OrderStatus;

namespace Shop.WebApp.Consumers;

public class OrderStatusChangedToCancelledConsumer(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToCancelledConsumer> logger)
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