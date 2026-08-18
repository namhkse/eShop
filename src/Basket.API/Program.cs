using Basket.API.Extensions;
using Basket.API.Grpc;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddBasicServiceDefaults();

builder.AddApplicationServices();

builder.Services.AddGrpc();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapGrpcService<BasketService>();

app.Run();