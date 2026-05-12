using MiniCommerce.OrderService.Models;

namespace MiniCommerce.OrderService.Services.Foundations;

public interface IOrderService
{
    ValueTask AddOrderAsync(Order order);
    ValueTask<IEnumerable<Order>> RetrieveAllOrdersAsync();
    ValueTask<Order?> RetrieveOrderByIdAsync(Guid id);
    ValueTask ModifyOrderAsync(Order order);
    ValueTask RemoveOrderByIdAsync(Guid id);
}
