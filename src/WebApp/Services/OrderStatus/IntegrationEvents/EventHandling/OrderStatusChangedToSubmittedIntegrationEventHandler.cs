using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shop.Contracts.Orders;

namespace Shop.WebApp.Services.OrderStatus.IntegrationEvents.EventHandling;

public class OrderStatusChangedToSubmittedIntegrationEventHandlerDefinition
    : ConsumerDefinition<OrderStatusChangedToSubmittedIntegrationEventHandler>
{
    public OrderStatusChangedToSubmittedIntegrationEventHandlerDefinition()
    {
        EndpointName = "web-app-order-status-changed-to-stock-confirmed-integration-event-handler";
    }
}

public class OrderStatusChangedToSubmittedIntegrationEventHandler(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToSubmittedIntegrationEventHandler> logger)
    : IConsumer<OrderStatusChangedToSubmittedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToSubmittedIntegrationEvent> context)
    {
        await orderStatusNotificationService.NotifyOrderStatusChangedAsync(context.Message.BuyerIdentityGuid);
    }
}