using Catalog.API;
using Catalog.API.Apis;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.Services.AddHealthChecks();
builder.Services.AddProblemDetails();

var app = builder.Build();
app.MapHealthChecks("/health");
app.MapCatalogApi();
app.Run();