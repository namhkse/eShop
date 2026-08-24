namespace OrderProcessor.Repositories;

public interface IGracePeriodOrdersRepository
{
    ValueTask<List<int>> GetConfirmedGracePeriodOrdersAsync(
        TimeSpan gracePeriod,
        CancellationToken cancellationToken);
}