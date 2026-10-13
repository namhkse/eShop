using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Application.Orders.QueryOrder;
using Ordering.API.Infrastructure;
using Ordering.API.Infrastructure.Services;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.OrderAggregate;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Idempotency;
using Ordering.Infrastructure.Repositories;
using Shop.ServiceDefaults;

namespace Ordering.API;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddDbContext<OrderingContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("OrderingDB")));
        
        // TODO: remove this
        builder.Services.AddMigration<OrderingContext, OrderingContextSeed>();

        services.AddHttpContextAccessor();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddScoped<IOrderQueries, OrderQueries>();
        services.AddScoped<IBuyerRepository, BuyerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRequestManager, RequestManager>();
        builder.AddDefaultAuthentication();

        builder.Services.AddMassTransit(x =>
        {
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("ordering"));
            x.AddConsumers(typeof(Program).Assembly);
            x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!));
                    cfg.ConfigureEndpoints(ctx);
                }
            );
        });
    }
}