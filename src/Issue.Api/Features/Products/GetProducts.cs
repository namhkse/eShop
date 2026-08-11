using Issue.Api.Endpoints;
using Issue.Api.Entities;

namespace Issue.Api.Features.Products;

public static class GetProducts
{
   public record Response(int Id, string Name, decimal Price);
   
   public sealed class Endpoint : IEndpoint
   {
      public void MapEndpoint(IEndpointRouteBuilder app)
      {
         app.MapGet("/products", Handler).WithTags("products");
      }
   }

   public static async Task<IResult> Handler()
   {
      var products = Enumerable.Repeat(new Product(), 1);
      
      var responses = products.Select(p => new Response(p.Id, p.Name, p.Price)); 
      
      return TypedResults.Ok(responses);
   }
}