using EventBusRabbitMQ;
using IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Apis;
using Ordering.API.Application;
using Ordering.API.Application.Behaviors;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Application.Queries;
using Ordering.API.Infrastructure;
using Ordering.API.Infrastructure.Services;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.OrderAggregate;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Idempotency;
using Ordering.Infrastructure.Repositories;
using Scalar.AspNetCore;
using Shared;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// The OpenAPI service
services.AddOpenApi();

// The database context
services.AddDbContext<OrderingContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderingDB"));
});

// Integration services
services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<OrderingContext>>();
services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

builder.AddRabbitMqEventBus("eventbus");
// TODO: .AddSubscription<OrderStock>()

services.AddHttpContextAccessor();
services.AddTransient<IIdentityService, IdentityService>();

services.AddScoped<IOrderQueries, OrderQueries>();
services.AddScoped<IBuyerRepository, BuyerRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IRequestManager, RequestManager>();

// Configure mediatR
services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapOrdersApi();

app.Run();