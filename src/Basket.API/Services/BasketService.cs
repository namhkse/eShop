using Basket.API.Models;
using Basket.API.Repositories;
using Grpc.Core;

namespace Basket.API.Services;

public class BasketService(IBasketRepository repository) : Basket.BasketBase
{
    public override async Task<CustomerBasketResponse> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        var userId = context.GetUserIdentity();

        if (string.IsNullOrEmpty(userId)) return new();

        var data = await repository.GetBasketAsync(userId);

        return MapToCustomerBasketResponse(data);
    }

    private static CustomerBasketResponse MapToCustomerBasketResponse(CustomerBasket customerBasket)
    {
        var response = new CustomerBasketResponse();

        foreach (var item in customerBasket.Items)
        {
            response.Items.Add(new BasketItem()
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
            });
        }

        return response;
    }
}