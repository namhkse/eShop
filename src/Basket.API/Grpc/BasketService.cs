using System.Diagnostics.CodeAnalysis;
using Basket.API.Models;
using Basket.API.Repositories;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Basket.API.Grpc;

public class BasketService(
    IBasketRepository repository,
    ILogger<BasketService> logger) : Basket.BasketBase
{
    [AllowAnonymous]
    public override async Task<CustomerBasketResponse> GetBasket(
        GetBasketRequest request,
        ServerCallContext context)
    {
        var userId = context.GetUserIdentity();

        if (string.IsNullOrEmpty(userId))
            return new();

        logger.LogInformation("Begin GetBasketById call from method {Method} for basket id {Id}",
            context.Method,
            userId);

        var data = await repository.GetBasketAsync(userId);

        if (data is null)
            ThrowBasketDoesNotExist(userId);

        return MapToCustomerBasketResponse(data);
    }

    public override async Task<CustomerBasketResponse> UpdateBasket(UpdateBasketRequest request,
        ServerCallContext context)
    {
        var userId = context.GetUserIdentity();

        if (string.IsNullOrEmpty(userId))
            ThrowNotAuthenticated();

        logger.LogInformation("Begin UpdateBasket call from method {Method} for basket id {Id}",
            context.Method,
            userId);

        var customerBasket = MapToCustomerBasket(userId, request);
        
        var response = await repository.UpdateBasketAsync(customerBasket);
        
        if (response is null) ThrowBasketDoesNotExist(userId);
        
        return MapToCustomerBasketResponse(response);
    }

    public override async Task<DeleteBasketResponse> DeleteBasket(DeleteBasketRequest request, ServerCallContext context)
    {
        var userId = context.GetUserIdentity();
        
        if (string.IsNullOrEmpty(userId)) ThrowNotAuthenticated();
        
        await repository.DeleteBasketAsync(userId);

        return new();
    }

    [DoesNotReturn]
    private static void ThrowBasketDoesNotExist(string userId) =>
        throw new RpcException(new Status(StatusCode.NotFound,
            $"Basket with buyer id {userId} does not exist"));

    [DoesNotReturn]
    private static void ThrowNotAuthenticated() =>
        throw new RpcException(new Status(StatusCode.Unauthenticated,
            "The caller is not authenticated"));

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

    private static CustomerBasket MapToCustomerBasket(string userId,
        UpdateBasketRequest request)
    {
        var response = new CustomerBasket
        {
            BuyerId = userId
        };

        foreach (var item in request.Items)
        {
            response.Items.Add(new()
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
            });
        }
        
        return response;
    }
}