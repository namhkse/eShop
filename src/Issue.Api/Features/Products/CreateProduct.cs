using FluentValidation;
using Issue.Api.Endpoints;
using Issue.Api.Entities;

namespace Issue.Api.Features.Products;

public class CreateProduct
{
    public record Request(string Name, decimal Price);
    public record Response(int Id, string Name, decimal Price);

    public sealed class Validator : AbstractValidator<Request>
    {
       public  Validator()
       {
           RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required");
           RuleFor(x => x.Price).GreaterThan(0).WithMessage("Product price must be greater than 0");
       } 
    }
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("products", Handler).WithTags("products");
        }
    }

    public static async Task<IResult> Handler(
        Request request,
        IValidator<Request> validator)
    {
        var validatorResult = await validator.ValidateAsync(request);

        if (!validatorResult.IsValid)
        {
            return Results.BadRequest(validatorResult.Errors);
        }
        
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        return Results.Ok(new Response(product.Id, product.Name, product.Price));
    }
}