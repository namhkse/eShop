using Catalog.API.Infrastructure;
using MassTransit;
using Ordering.Contracts;

namespace Catalog.API.Consumers;

public class OrderStatusChangedToPaidConsumer(
    CatalogContext catalogContext,
    ILogger<OrderStatusChangedToPaidConsumer> logger)
    : IConsumer<OrderStatusChangedToPaidIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderStatusChangedToPaidIntegrationEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})",
            context.MessageId,
            message);

        foreach (var orderStockItem in message.OrderStockItems)
        {
            var catalogItem = catalogContext.CatalogItems.Find(orderStockItem.ProductId);

            catalogItem?.RemoveStock(orderStockItem.Units);
        }

        await catalogContext.SaveChangesAsync();
    }
}