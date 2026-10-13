using System;
using System.Threading.Tasks;
using eShop.WebAppComponents.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Shop.ServiceDefaults;
using Shop.WebApp.Services;
using Shop.WebApp.Services.OrderStatus;

namespace Shop.WebApp.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddAuthenticationServices();

        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(Program).Assembly);
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("webapp"));

            x.UsingRabbitMq((ctx, cfg) => 
                {
                    cfg.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!));
                    cfg.ConfigureEndpoints(ctx);
                }
            );
        });

        builder.Services.AddHttpForwarderWithServiceDiscovery();

        // Application services
        builder.Services.AddScoped<BasketState>();
        builder.Services.AddScoped<LogOutService>();
        builder.Services.AddSingleton<BasketService>();
        builder.Services.AddSingleton<OrderStatusNotificationService>();
        builder.Services.AddSingleton<IProductImageUrlProvider, ProductImageUrlProvider>();
        builder.AddAIServices();

        // HTTP and GRPC client registrations
        builder.Services.AddGrpcClient<Basket.API.Basket.BasketClient>(o => o.Address = new Uri(builder.Configuration.GetRequiredValue("BasketAPI")))
            .AddAuthToken();
        
        builder.Services.AddHttpClient<CatalogService>(o => o.BaseAddress = new Uri(builder.Configuration.GetRequiredValue("CatalogAPI")))
            .AddApiVersion(2.0)
            .AddAuthToken();
        
        builder.Services.AddHttpClient<OrderingService>(o => o.BaseAddress = new Uri(builder.Configuration.GetRequiredValue("OrderingAPI")))
            .AddApiVersion(1.0)
            .AddAuthToken();
    }

    public static void AddAuthenticationServices(this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var services = builder.Services;

        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        var identityUrl = configuration.GetRequiredValue("IdentityUrl");
        var callBackUrl = configuration.GetRequiredValue("CallBackUrl");
        var sessionCookieLifetime = configuration.GetValue("SessionCookieLifetimeMinutes", 60);
        
        // Add Authentication services
        services.AddAuthorization();
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options => options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = identityUrl;
                options.SignedOutRedirectUri = callBackUrl;
                options.ClientId = "webapp";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.RequireHttpsMetadata = false;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("orders");
                options.Scope.Add("basket");
            });

        // Blazor auth services
        services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
        services.AddCascadingAuthenticationState();
    }

    private static void AddAIServices(this IHostApplicationBuilder builder)
    {
        ChatClientBuilder? chatClientBuilder = null;
        if (builder.Configuration["OllamaEnabled"] is string ollamaEnabled && bool.Parse(ollamaEnabled))
        {
            chatClientBuilder = builder.AddOllamaApiClient("chat")
                .AddChatClient();
        }
        else if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("chatModel")))
        {
            chatClientBuilder = builder.AddOpenAIClientFromConfiguration("chatModel")
                .AddChatClient();
        }

        chatClientBuilder?.UseFunctionInvocation();
    }

    public static async Task<string?> GetBuyerIdAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst("sub")?.Value;
    }

    public static async Task<string?> GetUserNameAsync(this AuthenticationStateProvider authenticationStateProvider)
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst("name")?.Value;
    }
}