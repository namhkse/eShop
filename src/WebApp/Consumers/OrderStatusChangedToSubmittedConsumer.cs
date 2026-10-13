using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Contracts;
using Shop.WebApp.Services.OrderStatus;

namespace Shop.WebApp.Consumers;

public class OrderStatusChangedToSubmittedConsumer(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToSubmittedConsumer> logger)
    : IConsumer<OrderStatusChangedToSubmittedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToSubmittedIntegrationEvent> context)
    {
        await orderStatusNotificationService.NotifyOrderStatusChangedAsync(context.Message.BuyerIdentityGuid);
    }
}