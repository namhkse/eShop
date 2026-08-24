using MassTransit;
using Microsoft.Extensions.Options;
using OrderProcessor.Repositories;
using Shop.Contracts.OrderProcessor;
using Shop.Contracts.Orders;

namespace OrderProcessor.Services;

public class GracePeriodManagerService(
    IOptions<BackgroundTaskOptions> backgroundTaskOptions,
    IServiceProvider serviceProvider,
    ILogger<GracePeriodManagerService> logger,
    IGracePeriodOrdersRepository repository) : BackgroundService
{
    private readonly BackgroundTaskOptions options = backgroundTaskOptions?.Value!;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delayTime = TimeSpan.FromSeconds(options.CheckUpdateTime);

        using var scope = serviceProvider.CreateScope();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckConfirmedGracePeriodOrders(publishEndpoint,
                stoppingToken);

            await Task.Delay(delayTime,
                stoppingToken);
        }
    }

    private async Task CheckConfirmedGracePeriodOrders(IPublishEndpoint publishEndpoint,
        CancellationToken cancellationToken = default)
    {
        var orderIds = await repository.GetConfirmedGracePeriodOrdersAsync(
            TimeSpan.FromSeconds(options.GracePeriodTime),
            cancellationToken);

        foreach (var orderId in orderIds)
        {
            await publishEndpoint.Publish(
                GracePeriodConfirmedIntegrationEvent.Create(orderId),
                cancellationToken);
        }
    }

}