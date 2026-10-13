using MediatR;

namespace Ordering.API.Application.Orders.CancelOrder;

public record CancelOrderCommand(int OrderNumber) : IRequest<bool>;