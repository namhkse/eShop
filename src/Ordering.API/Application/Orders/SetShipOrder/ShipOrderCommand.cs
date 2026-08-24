using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace Ordering.API.Application.Orders.SetShipOrder;

public record ShipOrderCommand(
    int OrderNumber) : IRequest<bool>,
    IRequest;