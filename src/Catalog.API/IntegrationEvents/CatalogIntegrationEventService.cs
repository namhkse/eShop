using Catalog.API.Infrastructure;
using EventBus;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.IntegrationEvents;

public class CatalogIntegrationEventService(
    IEventBus eventBus,
    CatalogContext catalogContext
) : ICatalogIntegrationEventService
{
    public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt)
    {
        // TODO:
        //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
        await ResilientTransaction.New(catalogContext).ExecuteAsync(async () =>
        {
            // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
           await catalogContext.SaveChangesAsync();
           // TODO: await integrationEventLogService.SaveEventAsync(evt, catalogContext.Database.CurrentTransaction);
        });
    }

    public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
    {
        await eventBus.PublishAsync(evt);
    }
}

public class ResilientTransaction
{
    private readonly DbContext _context;
    private ResilientTransaction(DbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public static ResilientTransaction New(DbContext context) => new(context);

    public async Task ExecuteAsync(Func<Task> action)
    {
        //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            await action();
            await transaction.CommitAsync();
        });
    }
}
