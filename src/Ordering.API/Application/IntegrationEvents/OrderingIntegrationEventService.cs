using EventBus;
using IntegrationEventLogEF.Services;
using Ordering.Infrastructure;

namespace Ordering.API.Application.IntegrationEvents;

public class OrderingIntegrationEventService(
    IEventBus eventBus,
    OrderingContext orderingContext,
    IIntegrationEventLogService integrationEventLogService,
    ILogger<OrderingIntegrationEventService> logger)
    : IOrderingIntegrationEventService
{
    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        var pendingLogEvents = await integrationEventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

        foreach (var evt in pendingLogEvents)
        {
            // TODO: Add log

            try
            {
                await integrationEventLogService.MarkEventAsInProgressAsync(evt.EventId);
                await eventBus.PublishAsync(evt.IntegrationEvent);
                await integrationEventLogService.MarkEventAsPublishedAsync(evt.EventId);
            }
            catch (Exception ex)
            {
                await integrationEventLogService.MarkEventAsPublishedAsync(evt.EventId);
            }
        }
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent evt)
    {
        // TODO: log
        await integrationEventLogService.SaveEventAsync(evt, orderingContext.GetCurrentTransaction());
    }
}