using System.Text.Json;
using EventBus;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EventBusRabbitMQ;

public sealed class RabbitMqEventBus(
    ILogger<RabbitMqEventBus> logger,
    IServiceProvider serviceProvider,
    IOptions<EventBusOptions> options,
    IOptions<EventBusSubscriptionInfo> subscriptionOptions
) : IEventBus, IDisposable, IHostedService
{
    private IConnection rabbitMqConnection;
    private IChannel consumerChannel;

    private const string ExchangeName = "eshop_event_bus";
    private readonly string queueName = options.Value.SubscriptionClientName;
    private readonly EventBusSubscriptionInfo _subscriptionInfo = subscriptionOptions.Value;

    public async Task PublishAsync(IntegrationEvent evt)
    {
        await using var channel = await rabbitMqConnection?.CreateChannelAsync()!;

        await channel.ExchangeDeclareAsync(ExchangeName, "direct");

        var routingKey = evt.GetType().Name;
        var body = SerializeMessage(evt);
        var properties = new BasicProperties()
        {
            DeliveryMode = DeliveryModes.Persistent
        };

        await channel.BasicPublishAsync(
            exchange: ExchangeName,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: properties,
            body: body);
    }

    private static byte[] SerializeMessage(IntegrationEvent evt)
    {
        return JsonSerializer.SerializeToUtf8Bytes(
            evt,
            evt.GetType(),
            JsonSerializerOptions.Default);
    }

    public Task StartAsync(CancellationToken ct)
    {
        Task.Factory.StartNew(async () =>
        {
            rabbitMqConnection = serviceProvider.GetRequiredService<IConnection>();

            if (!rabbitMqConnection.IsOpen) return;

            consumerChannel = await rabbitMqConnection.CreateChannelAsync(cancellationToken: ct);

            await consumerChannel.ExchangeDeclareAsync(
                exchange: ExchangeName,
                type: "direct",
                cancellationToken: ct);

            await consumerChannel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(consumerChannel);

            consumer.ReceivedAsync += OnMessageReceived;

            await consumerChannel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer);

            foreach (var (eventName, _) in _subscriptionInfo.EventTypes)
            {
                await consumerChannel.QueueBindAsync(
                    exchange: ExchangeName,
                    queue: queueName,
                    routingKey: eventName);
            }
        }, TaskCreationOptions.LongRunning);

        return Task.CompletedTask;
    }

    private Task OnMessageReceived(object sender, BasicDeliverEventArgs @event)
    {
        // TODO: 
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        consumerChannel?.Dispose();
    }
}