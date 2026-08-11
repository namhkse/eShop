using Carter;
using Catalog.API.Database;
using Catalog.API.Model;

namespace Catalog.API.Modules.Catalogs;

public static class CreateCatalog
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/catalogs/items", Handle).WithTags("Catalogs");
        }
    }
    
    public static async Task<IResult> Handle(
        CatalogItem product,
        CatalogContext catalogContext)
    {
        var item = new CatalogItem
        {
            CatalogBrandId = product.CatalogBrandId,
            CatalogTypeId = product.CatalogTypeId,
            Description = product.Description,
            Name = product.Name,
            PictureFileName = product.PictureFileName,
            Price = product.Price
        };

        catalogContext.CatalogItems.Add(item);

        await catalogContext.SaveChangesAsync();
        
        return TypedResults.CreatedAtRoute($"/api/items/{item.Id}", null);
    }
}