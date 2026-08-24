using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.API.Application.Orders.CreateOrderDraft;

public record CreateOrderDraftCommand(
    string BuyerId,
    IEnumerable<BasketItem> Items) : IRequest<OrderDraftDTO>;

public record OrderDraftDTO(
    IEnumerable<OrderItemDTO> OrderItems,
    decimal Total) 
{
    public static OrderDraftDTO FromOrder(Order order)
    {
        var items = order.OrderItems.Select(oi => new OrderItemDTO
        (
            oi.ProductId,
            oi.ProductName,
            oi.UnitPrice,
            oi.Discount,
            oi.Units,
            oi.PictureUrl
        ));

        return new OrderDraftDTO(items, order.GetTotal());
    }
}

public record OrderItemDTO(
    int ProductId,
    string ProductName,
    decimal UnitPrice,
    decimal Discount,
    int Units,
    string PictureUrl
);