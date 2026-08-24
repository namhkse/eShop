namespace Shop.Contracts.OrderProcessor;

public record GracePeriodConfirmedIntegrationEvent(
    Guid Id,
    DateTime OccurredAt,
    int OrderId)
{
    public static GracePeriodConfirmedIntegrationEvent Create(int orderId)
        => new(
            Guid.NewGuid(),
            DateTime.UtcNow,
            orderId);
}