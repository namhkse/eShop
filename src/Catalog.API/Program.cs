using Catalog.API;
using Catalog.API.Apis;
using Catalog.API.Infrastructure;
using Catalog.API.IntegrationEvents.EventHandling;
using Catalog.API.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<CatalogContext>("catalogdb",
    configureDbContextOptions: options => options.UseNpgsql(b => { b.UseVector(); }));

builder.Services.AddProblemDetails();
builder.Services.AddOptions<CatalogOptions>().BindConfiguration(nameof(CatalogOptions));
builder.Services.AddScoped<ICatalogAI, CatalogAI>();

if (builder.Configuration["OllamaEnabled"] is string ollamaEnabled && bool.Parse(ollamaEnabled))
{
    builder.AddOllamaApiClient("embedding").AddEmbeddingGenerator();
}

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumer<OrderStatusChangedToPaidIntegrationEventHandler>();
    busConfigurator.AddConsumer<OrderStatusChangedToAwaitingValidationIntegrationEventHandler, OrderStatusChangedToAwaitingValidationConsumerDefinition>();
    busConfigurator.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

app.MapCatalogApi();

app.Run();