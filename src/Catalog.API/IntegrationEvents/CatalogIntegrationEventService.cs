using Catalog.API.Infrastructure;
using EventBus;
using IntegrationEventLogEF.Services;
using IntegrationEventLogEF.Utilities;

namespace Catalog.API.IntegrationEvents;

public class CatalogIntegrationEventService(
    IEventBus eventBus,
    CatalogContext catalogContext,
    IIntegrationEventLogService integrationEventLogService
) : ICatalogIntegrationEventService
{
    public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt)
    {
        // TODO: add log
        await ResilientTransaction.New(catalogContext).ExecuteAsync(async () =>
        {
            await catalogContext.SaveChangesAsync();
            await integrationEventLogService.SaveEventAsync(evt, catalogContext.Database.CurrentTransaction);
        });
    }

    public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
    {
        try
        {
            // TODO: add log
            await integrationEventLogService.MarkEventAsInProgressAsync(evt.Id);
            await eventBus.PublishAsync(evt);
            await integrationEventLogService.MarkEventAsPublishedAsync(evt.Id);
        }
        catch (Exception ex)
        {
            // TODO: add log
            await integrationEventLogService.MarkEventAsPublishedAsync(evt.Id);
        }
    }
}