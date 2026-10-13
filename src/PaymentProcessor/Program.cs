using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<PaymentOptions>().BindConfiguration(nameof(PaymentOptions));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(Program).Assembly);
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("payment-processor"));
    x.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!));
            cfg.ConfigureEndpoints(ctx);
        }
    );
});

var app = builder.Build();

app.Run();