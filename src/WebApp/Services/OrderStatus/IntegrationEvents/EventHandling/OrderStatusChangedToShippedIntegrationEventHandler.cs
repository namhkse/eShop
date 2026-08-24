using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shop.Contracts.Orders;

namespace Shop.WebApp.Services.OrderStatus.IntegrationEvents.EventHandling;

public class OrderStatusChangedToShippedIntegrationEventHandler(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToShippedIntegrationEventHandler> logger)
    : IConsumer<OrderStatusChangedToShippedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToShippedIntegrationEvent> context)
    {
        await orderStatusNotificationService.NotifyOrderStatusChangedAsync(context.Message.BuyerIdentityGuid);
    }
}