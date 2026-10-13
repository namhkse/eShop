using Microsoft.EntityFrameworkCore;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.SeedWork;

namespace Ordering.Infrastructure.Repositories;

public class BuyerRepository(OrderingContext context) : IBuyerRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Buyer Add(Buyer buyer)
    {
        if (buyer.IsTransient())
        {
            return context.Buyers.Add(buyer).Entity;
        }

        return buyer;
    }

    public Buyer Update(Buyer buyer)
    {
        return context.Buyers.Update(buyer).Entity;
    }

    public async Task<Buyer> FindAsync(string identity)
    {
        var buyer = await context.Buyers
            .Include(b => b.PaymentMethods)
            .Where(b => b.IdentityGuid == identity)
            .SingleOrDefaultAsync();

        return buyer;
    }

    public async Task<Buyer> FindByIdAsync(int id)
    {
        var buyer = await context.Buyers
            .Include(b => b.PaymentMethods)
            .Where(b => b.Id == id)
            .SingleOrDefaultAsync();

        return buyer;
    }
}