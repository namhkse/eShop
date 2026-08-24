using MediatR;

namespace Ordering.API.Application.Orders.SetStockRejectedOrderStatus;

public record SetStockRejectedOrderStatusCommand(int OrderNumber, List<int> OrderStockItems) : IRequest<bool>;