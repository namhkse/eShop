using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shop.Contracts.Orders;

namespace Shop.WebApp.Services.OrderStatus.IntegrationEvents.EventHandling;

public class OrderStatusChangedToStockConfirmedIntegrationEventHandlerDefinition
    : ConsumerDefinition<OrderStatusChangedToStockConfirmedIntegrationEventHandler>
{
    public OrderStatusChangedToStockConfirmedIntegrationEventHandlerDefinition()
    {
        EndpointName = "web-app-order-status-changed-to-stock-confirmed-integration-event-handler";
    }
}

public class OrderStatusChangedToStockConfirmedIntegrationEventHandler(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger)
    : IConsumer<OrderStatusChangedToStockConfirmedIntegrationEvent>
{

    public async Task Consume(ConsumeContext<OrderStatusChangedToStockConfirmedIntegrationEvent> context)
    {
        logger.LogInformation("Foo Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            context.Message.GetType().Name);
        
        await orderStatusNotificationService.NotifyOrderStatusChangedAsync(context.Message.BuyerIdentityGuid);
    }
}
