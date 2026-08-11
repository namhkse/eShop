using Carter;
using Catalog.API.Database;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Modules.Dev;

public static class SeedData
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/seeds", Handler).WithTags("Seeds");
        }
    }

     private static async Task<IResult> Handler(
        CatalogContext context,
        IWebHostEnvironment env,
        IOptions<CatalogSettings> settings,
        ILogger<CatalogContextSeed> logger)
    {
        // await context.Database.MigrateAsync();
        //
        // await new CatalogContextSeed().SeedAsync(context, env, settings, logger);

        return Results.Ok();
    }
}