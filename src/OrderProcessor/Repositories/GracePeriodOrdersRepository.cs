using Dapper;
using Npgsql;

namespace OrderProcessor.Repositories;

internal sealed class GracePeriodOrdersRepository(
    NpgsqlDataSource dataSource,
    ILogger<GracePeriodOrdersRepository> logger) : IGracePeriodOrdersRepository
{
    public async ValueTask<List<int>> GetConfirmedGracePeriodOrdersAsync(
        TimeSpan gracePeriod,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

            const string sql = """
                SELECT "Id"
                FROM ordering.orders
                WHERE CURRENT_TIMESTAMP - "OrderDate" >= @GracePeriodTime
                  AND "OrderStatus" = 'Submitted'
                """;

            var ids = await connection.QueryAsync<int>(
                new CommandDefinition(
                    sql,
                    new { GracePeriodTime = gracePeriod },
                    cancellationToken: cancellationToken));

            return ids.AsList();
        }
        catch (NpgsqlException exception)
        {
            logger.LogError(
                exception,
                "Error querying confirmed grace period orders after grace period {GracePeriod}",
                gracePeriod);

            return [];
        }
    }
}