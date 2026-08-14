using Microsoft.EntityFrameworkCore;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.SeedWork;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository(OrderingContext context) : IOrderRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Order Add(Order order)
    {
        return context.Orders.Add(order).Entity;
    }

    public async Task<Order> GetAsync(int orderId)
    {
        var order = await context.Orders.FindAsync(orderId);

        if (order != null)
        {
            await context.Entry(order).Collection(i => i.OrderItems).LoadAsync();
        }

        return order;
    }

    public void Update(Order order)
    {
        context.Entry(order).State = EntityState.Modified;
    }
}