using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.ServiceDefaults;
using Shop.WebApp.Components;
using Shop.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseAntiforgery();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
var catalogApi = builder.Configuration.GetRequiredValue("CatalogAPI");
app.MapForwarder("/product-images/{id}", catalogApi, "/api/catalog/items/{id}/pic");
app.Run();
