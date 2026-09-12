using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shop.Contracts.Orders;

namespace Shop.WebApp.Services.OrderStatus.IntegrationEvents.EventHandling;

public class OrderStatusChangedToPaidIntegrationEventHandlerDefinition 
    : ConsumerDefinition<OrderStatusChangedToPaidIntegrationEventHandler>
{
    public OrderStatusChangedToPaidIntegrationEventHandlerDefinition()
    {
        EndpointName = "web-app-order-status-changed-to-paid-integration-event-handler";
    }
}
    
public class OrderStatusChangedToPaidIntegrationEventHandler(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger)
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