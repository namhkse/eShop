using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shop.Contracts.Orders;

namespace Shop.WebApp.Services.OrderStatus.IntegrationEvents.EventHandling;

public class OrderStatusChangedToStockConfirmedIntegrationEventHandler(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger)
    : IConsumer<OrderStatusChangedToStockConfirmedIntegrationEvent>
{

    public async Task Consume(ConsumeContext<OrderStatusChangedToStockConfirmedIntegrationEvent> context)
    {
        await orderStatusNotificationService.NotifyOrderStatusChangedAsync(context.Message.BuyerIdentityGuid);
    }
}
