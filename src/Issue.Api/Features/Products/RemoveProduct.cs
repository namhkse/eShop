using Issue.Api.Endpoints;

namespace Issue.Api.Features.Products;

public static class RemoveProduct
{
    public class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", Handler).WithTags("products");
        }
    }

    public static IResult Handler(int id)
    {
        // TODO: delete the product
        return Results.NoContent();
    }
}