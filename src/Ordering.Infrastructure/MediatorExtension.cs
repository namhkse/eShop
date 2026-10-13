using MediatR;
using Ordering.Domain.SeedWork;

namespace Ordering.Infrastructure;

static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderingContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var ls = new List<IReadOnlyCollection<INotification>>();

        foreach (var entity in domainEntities)
        {
            ls.Add(entity.Entity.DomainEvents);
        }

        var domainEvents = ls.SelectMany(l => l).ToList();

        
        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());
        
        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}