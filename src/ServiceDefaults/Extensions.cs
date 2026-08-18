using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceDefaults;

public static class Extensions
{
    public static IHostApplicationBuilder AddBasicServiceDefaults(this IHostApplicationBuilder builder)
    {
        // TODO: Add health check
        // TODO: Add open telemetry
        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // TODO: map /health and /alive APIs
        return app;
    }
    
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.AddBasicServiceDefaults();
        
        builder.Services.AddServiceDiscovery();
        
        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();
        
            // Turn on service discovery by default
            http.AddServiceDiscovery();
        });

        return builder;
    }
}