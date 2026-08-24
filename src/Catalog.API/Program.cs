using Catalog.API;
using Catalog.API.Apis;
using Catalog.API.Infrastructure;
using Catalog.API.IntegrationEvents.EventHandling;
using Catalog.API.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Shop.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.AddNpgsqlDbContext<CatalogContext>("CatalogDB",
    configureDbContextOptions: dbContextOptionsBuilder =>
    {
        dbContextOptionsBuilder.UseNpgsql(b => { b.UseVector(); });
    });

builder.Services.AddOptions<CatalogOptions>().BindConfiguration(nameof(CatalogOptions));
builder.Services.AddScoped<ICatalogAI, CatalogAI>();

if (builder.Configuration["OllamaEnabled"] is string ollamaEnabled && bool.Parse(ollamaEnabled))
{
    builder.AddOllamaApiClient("embedding").AddEmbeddingGenerator();
}

builder.Services.AddMassTransit(busConfigurator =>
{
    var rabbitMqSettings = builder.Configuration
        .GetSection(nameof(RabbitMqSettings))
        .Get<RabbitMqSettings>()!;

    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<OrderStatusChangedToPaidIntegrationEventHandler>();
    busConfigurator.AddConsumer<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();

    busConfigurator.UsingRabbitMq((ctx,
            cfg) =>
        {
            cfg.Host(rabbitMqSettings.Uri,
                "/",
                h =>
                {
                    h.Username(rabbitMqSettings.UserName);
                    h.Password(rabbitMqSettings.Password);
                });

            cfg.ConfigureEndpoints(ctx);
        }
    );
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapCatalogApi();

app.Run();