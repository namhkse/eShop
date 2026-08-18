using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceDefaults;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddDefaultAuthentication(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        
        var identitySection = configuration.GetSection("Identity");
        
        if (!identitySection.Exists()) return services;

        services.AddAuthentication().AddJwtBearer(options =>
        {
            var identityUrl = identitySection.GetRequiredValue("Url");
            var audience = identitySection.GetRequiredValue("Audience");
            
            options.Authority = identityUrl;
            options.RequireHttpsMetadata = false;
            options.Audience = audience;
            options.MapInboundClaims = false;
            
            options.TokenValidationParameters.ValidIssuers = [identityUrl];
            options.TokenValidationParameters.ValidateAudience = false;
        });
        
        services.AddAuthorization();
        
        return services;
    }
    
}