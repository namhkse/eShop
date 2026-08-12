using Microsoft.Extensions.DependencyInjection;

namespace EventBus;

public interface IEventBusBuilder
{
    public IServiceCollection Services { get; }
}