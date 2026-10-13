using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Contracts;
using Shop.WebApp.Services.OrderStatus;

namespace Shop.WebApp.Consumers;
    
public class OrderStatusChangedToPaidConsumer(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToPaidConsumer> logger)
    : IConsumer<OrderStatusChangedToPaidIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToPaidIntegrationEvent> context)
    {
        var @event = context.Message;
        
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            @event);
        
        await orderStatusNotificationService.NotifyOrderStatusChangedAsync(@event.BuyerIdentityGuid);
    }
}