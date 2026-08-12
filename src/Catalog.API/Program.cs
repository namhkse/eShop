using Carter;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// TODO: builder.Services.AddOpenApi();
builder.Services.AddCarter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // TODO: app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapCarter();

app.Run();