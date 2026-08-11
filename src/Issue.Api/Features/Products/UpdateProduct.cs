using Issue.Api.Endpoints;
using Issue.Api.Entities;

namespace Issue.Api.Features.Products;

public static class UpdateProduct
{
    public record Request(string Name, decimal Price);
    public record Response(int Id, string Name, decimal Price);
   
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("/products/{id}", Handler).WithTags("products");
        }
    }

    public static IResult Handler(int id, Request request)
    {
        var product = new Product
        {
            Id = id,
            Name = request.Name,
            Price = request.Price
        };

        if (product is null)
        {
            return Results.NotFound();
        }
      
        return Results.Ok(new Response(product.Id, product.Name, product.Price));
    }
}