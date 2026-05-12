using Microsoft.EntityFrameworkCore;
using MiniCommerce.OrderService.Brokers.Storages;
using MiniCommerce.OrderService.Models;

namespace MiniCommerce.OrderService.Services.Foundations;
public class OrderService(IStorageBroker storageBroker) : IOrderService
{
    public async ValueTask AddOrderAsync(Order order)
    {
        if (order.Quantity <= 0)
            throw new InvalidQuantityException();

        order.Status = OrderStatus.Pending;
        await storageBroker.InsertOrderAsync(order);
    }

    public async ValueTask<IEnumerable<Order>> RetrieveAllOrdersAsync()
    {
        var ordersQuery = await storageBroker.SelectAllOrdersAsync();
        return await ordersQuery.ToListAsync();
    }

    public async ValueTask<Order?> RetrieveOrderByIdAsync(Guid id) =>
        await storageBroker.SelectOrderByIdAsync(id);

    public async ValueTask ModifyOrderAsync(Order order)
    {
        var existingOrder = await storageBroker.SelectOrderByIdAsync(order.Id);
        if (existingOrder is null)
            throw new OrderNotFoundException();

        await storageBroker.UpdateOrderAsync(order);
    }

    public async ValueTask RemoveOrderByIdAsync(Guid id)
    {
        var order = await storageBroker.SelectOrderByIdAsync(id);
        if (order is null)
            throw new OrderNotFoundException();

        await storageBroker.DeleteOrderAsync(order);
    }
}
