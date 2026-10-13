using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Contracts;
using Shop.WebApp.Services.OrderStatus;

namespace Shop.WebApp.Consumers;

public class OrderStatusChangedToStockConfirmedConsumer(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToStockConfirmedConsumer> logger)
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
