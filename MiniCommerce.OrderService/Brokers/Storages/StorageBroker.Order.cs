using MiniCommerce.OrderService.Models;

namespace MiniCommerce.OrderService.Brokers.Storages;

public partial class StorageBroker
{
    public async ValueTask InsertOrderAsync(Order order)
    {
        await this.Orders.AddAsync(order);
        await this.SaveChangesAsync();
    }

    public async ValueTask<IQueryable<Order>> SelectAllOrdersAsync() =>
        await ValueTask.FromResult(this.Orders.AsQueryable());

    public async ValueTask<Order?> SelectOrderByIdAsync(Guid id) =>
        await this.Orders.FindAsync(id);

    public async ValueTask UpdateOrderAsync(Order order)
    {
        this.Orders.Update(order);
        await this.SaveChangesAsync();
    }

    public async ValueTask DeleteOrderAsync(Order order)
    {
        this.Orders.Remove(order);
        await this.SaveChangesAsync();
    }
}
