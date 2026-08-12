using Microsoft.Extensions.DependencyInjection;

namespace EventBus;

public static class EventBusBuilderExtensions
{
    public static IEventBusBuilder AddSubscription<T>(IEventBusBuilder builder)
        where T: IntegrationEvent
    {
        // TODO: register handler
        
        builder.Services.Configure<EventBusSubscriptionInfo>(o =>
        {
            // Keep track registered event types.
            o.EventTypes[typeof(T).Name] = typeof(T);
        });
        
        return builder;
    }
}