using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shop.Contracts.Orders;

namespace Shop.WebApp.Services.OrderStatus.IntegrationEvents.EventHandling;

public class OrderStatusChangedToAwaitingValidationConsumerDefinition
    : ConsumerDefinition<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>
{
    public OrderStatusChangedToAwaitingValidationConsumerDefinition()
    {
        EndpointName = "webapp-order-status-changed-to-awaiting-validation";
    }
}

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