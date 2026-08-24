using Basket.API.Grpc;
using Basket.API.IntegrationEvents.EventHandling;
using Basket.API.Repositories;
using MassTransit;
using ServiceDefaults;
using Shop.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.AddDefaultAuthentication();
builder.AddRedisClient("redis");
builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();

builder.Services.AddMassTransit(busConfigurator =>
{
    var rabbitMqSettings = builder.Configuration
        .GetSection(nameof(RabbitMqSettings))
        .Get<RabbitMqSettings>()!;

    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<OrderStartedIntegrationEventHandler>();

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

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<BasketService>();

app.Run();