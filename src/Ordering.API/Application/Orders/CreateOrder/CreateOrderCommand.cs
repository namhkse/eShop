using MediatR;
using Ordering.API.Application.Orders.CreateOrderDraft;

namespace Ordering.API.Application.Orders.CreateOrder;

public record CreateOrderCommand(
    List<OrderItemDTO> OrderItems,
    string UserId,
    string UserName,
    string City,
    string Street,
    string State,
    string Country,
    string ZipCode,
    string CardNumber,
    string CardHolderName,
    DateTime CardExpiration,
    string CardSecurityNumber,
    int CardTypeId) : IRequest<bool>;