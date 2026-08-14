using EventBus;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationEventLogEF.Services;

public interface IIntegrationEventLogService
{
    Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId);
    
    Task SaveEventAsync(IntegrationEvent evt, IDbContextTransaction transaction);
    
    Task MarkEventAsPublishedAsync(Guid eventId);
    
    Task MarkEventAsInProgressAsync(Guid eventId);
    
    Task MarkEventAsFailedAsync(Guid eventId);
}