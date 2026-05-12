using MiniCommerce.OrderService.Models;

namespace MiniCommerce.OrderService.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask InsertOrderAsync(Order order);
    ValueTask<IQueryable<Order>> SelectAllOrdersAsync();
    ValueTask<Order?> SelectOrderByIdAsync(Guid id);
    ValueTask UpdateOrderAsync(Order order);
    ValueTask DeleteOrderAsync(Order order);
}
