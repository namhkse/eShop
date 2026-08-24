using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shop.Contracts.Orders;

namespace Shop.WebApp.Services.OrderStatus.IntegrationEvents.EventHandling;

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