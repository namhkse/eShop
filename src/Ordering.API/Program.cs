using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Apis;
using Ordering.API.Application.Behaviors;
using Ordering.API.Application.IntegrationEvents.EventHandling;
using Ordering.API.Application.Orders.QueryOrder;
using Ordering.API.Infrastructure.Services;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.OrderAggregate;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Idempotency;
using Ordering.Infrastructure.Repositories;
using Shop.Contracts;
using Shop.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// The database context
services.AddDbContext<OrderingContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderingDB")));

// TODO: builder.Services.AddMigration<OrderingContext, OrderingContextSeed>();

services.AddHttpContextAccessor();
services.AddTransient<IIdentityService, IdentityService>();

services.AddScoped<IOrderQueries, OrderQueries>();
services.AddScoped<IBuyerRepository, BuyerRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IRequestManager, RequestManager>();

builder.AddDefaultAuthentication();

builder.Services.AddMassTransit(busConfigurator =>
{
    var rabbitMqSettings = builder.Configuration
        .GetSection(nameof(RabbitMqSettings))
        .Get<RabbitMqSettings>()!;

    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<GracePeriodConfirmedIntegrationEventHandler>();
    busConfigurator.AddConsumer<OrderPaymentFailedIntegrationEventHandler>();
    busConfigurator.AddConsumer<OrderPaymentSucceededIntegrationEventHandler>();
    busConfigurator.AddConsumer<OrderStockConfirmedIntegrationEventHandler>();
    busConfigurator.AddConsumer<OrderStockRejectedIntegrationEventHandler>();

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

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

var app = builder.Build();

app.MapOrdersApiV1();

app.Run();