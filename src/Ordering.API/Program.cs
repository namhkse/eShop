using Ordering.API;
using Ordering.API.Apis;
using Ordering.API.Application.Behaviors;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

var app = builder.Build();
app.MapOrdersApiV1();
app.Run();