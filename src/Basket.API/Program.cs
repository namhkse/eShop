using Basket.API.Grpc;
using Basket.API.IntegrationEvents.EventHandling;
using Basket.API.Repositories;
using MassTransit;
using Shop.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.AddRedisClient("redis");
builder.AddDefaultAuthentication();
builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("basket"));
    busConfigurator.AddConsumer<OrderStartedIntegrationEventHandler>();
    busConfigurator.UsingRabbitMq((ctx, cfg) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("rabbitmq");
        cfg.Host(connectionString);
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<BasketService>();

app.Run();