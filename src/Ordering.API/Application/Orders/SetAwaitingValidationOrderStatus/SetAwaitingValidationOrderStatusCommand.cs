using MediatR;

namespace Ordering.API.Application.Commands;

public record SetAwaitingValidationOrderStatusCommand(int OrderNumber) : IRequest<bool>;

// Regular CommandHandler

// Use for Idempotency in Command process