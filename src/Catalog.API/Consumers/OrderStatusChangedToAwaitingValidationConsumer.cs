using Basket.Contracts;
using Catalog.API.Infrastructure;
using MassTransit;
using Ordering.Contracts;

namespace Catalog.API.Consumers;

public class OrderStatusChangedToAwaitingValidationConsumer(
    CatalogContext catalogContext,
    IPublishEndpoint publishEndpoint,
    ILogger<OrderStatusChangedToAwaitingValidationConsumer> logger)
    : IConsumer<OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToAwaitingValidationIntegrationEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            message);

        var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();

        // Check the amount of each item in the warehouse.
        foreach (var orderStockItem in message.OrderStockItems)
        {
            var catalogItem = catalogContext.CatalogItems.Find(orderStockItem.ProductId);

            if (catalogItem is not null)
            {
                var hasStock = catalogItem.AvailableStock >= orderStockItem.Units;
                var confirmedOrderStockItem = new ConfirmedOrderStockItem(catalogItem.Id,
                    hasStock);
                confirmedOrderStockItems.Add(confirmedOrderStockItem);
            }
        }

        if (confirmedOrderStockItems.Any(c => !c.HasStock))
        {
            // Amount is not enough
            logger.LogInformation("Send OrderStockRejectedIntegrationEvent");

            await publishEndpoint.Publish(new OrderStockRejectedIntegrationEvent(message.OrderId,
                confirmedOrderStockItems));
        }
        else
        {
            // Amount is enough
            logger.LogInformation("Send OrderStockConfirmedIntegrationEvent");

            await publishEndpoint.Publish(new OrderStockConfirmedIntegrationEvent(message.OrderId));
        }
    }
}