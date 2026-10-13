using Ordering.Domain.SeedWork;

namespace Ordering.Domain.BuyerAggregate;

public interface IBuyerRepository : IRepository<Buyer>
{
    Buyer Add(Buyer buyer);
    Buyer Update(Buyer buyer);
    Task<Buyer> FindAsync(string BuyerIdentityGuid);
    Task<Buyer> FindByIdAsync(int id);
}