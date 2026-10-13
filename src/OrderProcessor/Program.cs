using MassTransit;
using OrderProcessor;
using OrderProcessor.Repositories;
using OrderProcessor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((ctx, cfg) => 
        {
            cfg.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!));
            cfg.ConfigureEndpoints(ctx);
        }
    );
});

builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("OrderingDB")!);

builder.Services.AddOptions<BackgroundTaskOptions>()
    .BindConfiguration(nameof(BackgroundTaskOptions));

builder.Services.AddSingleton<IGracePeriodOrdersRepository, GracePeriodOrdersRepository>();

builder.Services.AddHostedService<GracePeriodManagerService>();

var app = builder.Build();

app.Run();