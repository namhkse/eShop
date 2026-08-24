using MediatR;

namespace Ordering.API.Application.Orders.SetStockConfirmedOrderStatus;

public record SetStockConfirmedOrderStatusCommand(int OrderNumber) : IRequest<bool>;