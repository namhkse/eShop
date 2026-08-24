using MassTransit;
using OrderProcessor;
using OrderProcessor.Repositories;
using OrderProcessor.Services;
using Shop.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(busConfigurator =>
{
    var rabbitMqSettings = builder.Configuration
        .GetSection(nameof(RabbitMqSettings))
        .Get<RabbitMqSettings>()!;
    
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((ctx, cfg) => 
        {
            cfg.Host(rabbitMqSettings.Uri, "/", h =>
            {
                h.Username(rabbitMqSettings.UserName);
                h.Password(rabbitMqSettings.Password);
            });
            
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