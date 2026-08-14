using System.Reflection;
using EventBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationEventLogEF.Services;

public class IntegrationEventLogService<TContext> : IIntegrationEventLogService 
    where TContext : DbContext
{
    
    private readonly TContext context;
    private readonly Type[] eventTypes;

    public IntegrationEventLogService(TContext context)
    {
        this.context = context;
        eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
            .GetTypes()
            .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
            .ToArray();
    }
    
    public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId)
    {
        var result = await context.Set<IntegrationEventLogEntry>()
            .Where(e => e.TransactionId == transactionId && e.State == EventStateEnum.NotPublished)
            .ToListAsync();

        if (result.Count != 0)
        {
            return result.OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializedJsonContent(eventTypes.FirstOrDefault(t => t.Name == e.EventTypeShortName)));
        }
        
        return [];
    }

    public Task SaveEventAsync(IntegrationEvent evt, IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        
        var eventLogEntry = new IntegrationEventLogEntry(evt, transaction.TransactionId);
        
        context.Database.UseTransaction(transaction.GetDbTransaction());
        context.Set<IntegrationEventLogEntry>().Add(eventLogEntry);
        
        return context.SaveChangesAsync();
    }

    public Task MarkEventAsPublishedAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventStateEnum.Published);
    }

    public Task MarkEventAsInProgressAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventStateEnum.InProgress);
    }

    public Task MarkEventAsFailedAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
    }

    private Task UpdateEventStatus(Guid eventId, EventStateEnum status)
    {
        var eventLogEntry = context.Set<IntegrationEventLogEntry>()
            .Single(e => e.EventId == eventId);
        
        eventLogEntry.State = status;

        if (status == EventStateEnum.InProgress) eventLogEntry.TimeSent++;

        return context.SaveChangesAsync();
    }
}