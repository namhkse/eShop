using Catalog.API.Infrastructure;
using Catalog.API.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shop.ServiceDefaults;

namespace Catalog.API;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<CatalogContext>("CatalogDB", configureDbContextOptions: options => options.UseNpgsql(b => { b.UseVector(); }));
        builder.Services.AddOptions<CatalogOptions>().BindConfiguration(nameof(CatalogOptions));
        builder.Services.AddScoped<ICatalogAI, CatalogAI>();
        
        builder.Services.AddMigration<CatalogContext, CatalogContextSeed>();

        if (builder.Configuration["OllamaEnabled"] is string ollamaEnabled && bool.Parse(ollamaEnabled))
        {
            builder.AddOllamaApiClient("embedding").AddEmbeddingGenerator();
        }

        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(Program).Assembly);
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("catalog"));
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!));
                cfg.ConfigureEndpoints(ctx);
            });
        });
    }
}