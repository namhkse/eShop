using MediatR;

namespace Ordering.API.Application.Orders.SetPaidOrderStatus;

public record SetPaidOrderStatusCommand(int OrderNumber) : IRequest<bool>;