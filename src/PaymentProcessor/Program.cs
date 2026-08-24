using MassTransit;
using PaymentProcessor.IntegrationEvents.EventHandling;
using Shop.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(busConfigurator =>
{
    var rabbitMqSettings = builder.Configuration
        .GetSection(nameof(RabbitMqSettings))
        .Get<RabbitMqSettings>()!;

    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<OrderStatusChangedToStockConfirmedIntegrationEventHandler>();

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


builder.Services.AddOptions<PaymentOptions>()
    .BindConfiguration(nameof(PaymentOptions));

var app = builder.Build();

app.Run();