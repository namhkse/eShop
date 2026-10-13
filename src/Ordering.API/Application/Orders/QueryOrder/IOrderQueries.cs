namespace Ordering.API.Application.Orders.QueryOrder;

public interface IOrderQueries
{
    Task<Order> GetOrderAsync(int id);

    Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(string userId);

    Task<IEnumerable<CardType>> GetCardTypesAsync();
}