using Ordering.Domain.Exceptions;

namespace Ordering.Infrastructure.Idempotency;

public class RequestManager(OrderingContext context) : IRequestManager
{
    public async Task<bool> ExistAsync(Guid id)
    {
        return await context.FindAsync<ClientRequest>(id) != null;
    }

    public async Task CreateRequestForCommandAsync<T>(Guid id)
    {
        var exists = await ExistAsync(id);
        
        var request = exists
            ? throw new OrderingDomainException($"Request with id {id} already exists")
            : new ClientRequest
            {
                Id = id,
                Name = typeof(T).Name,
                Time = DateTime.UtcNow
            };
        
        context.Add(request);
        
        await context.SaveChangesAsync();
    }
}