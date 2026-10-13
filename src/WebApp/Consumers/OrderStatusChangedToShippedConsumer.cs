using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Contracts;
using Shop.WebApp.Services.OrderStatus;

namespace Shop.WebApp.Consumers;

public class OrderStatusChangedToShippedConsumer(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToShippedConsumer> logger)
    : IConsumer<OrderStatusChangedToShippedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToShippedIntegrationEvent> context)
    {
        await orderStatusNotificationService.NotifyOrderStatusChangedAsync(context.Message.BuyerIdentityGuid);
    }
}