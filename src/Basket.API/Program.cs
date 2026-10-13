using Basket.API.Grpc;
using Basket.API.Repositories;
using MassTransit;
using Shop.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.AddRedisClient("redis");
builder.AddDefaultAuthentication();

builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(Program).Assembly);
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("basket"));
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!));
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<BasketService>();
app.Run();