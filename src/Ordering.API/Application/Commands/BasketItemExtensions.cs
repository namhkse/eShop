using Ordering.API.Application.Orders.CreateOrderDraft;

namespace Ordering.API.Application.Commands;

public static class BasketItemExtensions
{
    public static IEnumerable<OrderItemDTO> ToOrderItemsDTO(this IEnumerable<BasketItem> basketItems)
    {
        foreach (var item in basketItems)
        {
            yield return item.ToOrderItemDTO();
        }
    }

    public static OrderItemDTO ToOrderItemDTO(this BasketItem item)
    {
        return new OrderItemDTO(
            item.ProductId,
            item.ProductName,
            item.UnitPrice,
            0,
            item.Quantity,
            item.PictureUrl);
    }
}