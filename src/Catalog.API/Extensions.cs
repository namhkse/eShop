using Catalog.API.Infrastructure;
using Catalog.API.IntegrationEvents;
using Catalog.API.Services;
using EventBusRabbitMQ;
using IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<CatalogContext>("CatalogDB",
            configureDbContextOptions: dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder.UseNpgsql(builder => { builder.UseVector(); });
            });


        builder.Services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<CatalogContext>>();

        builder.Services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

        builder.AddRabbitMqEventBus("eventbus");

        builder.Services.AddOptions<CatalogOptions>().BindConfiguration(nameof(CatalogOptions));

        if (builder.Configuration["OllamaEnabled"] is string ollamaEnabled && bool.Parse(ollamaEnabled))
        {
            builder.AddOllamaApiClient("embedding")
                .AddEmbeddingGenerator();
        }

        builder.Services.AddScoped<ICatalogAI, CatalogAI>();
    }

    public static void AddSeeding(this IHostApplicationBuilder builder)
    {
        // This is done for development ease but shouldn't be here in production
        // builder.Services.AddMigration<CatalogContext, CatalogContextSeed>();
    }
}