using Microsoft.EntityFrameworkCore;
using MiniCommerce.OrderService.Brokers.Storages;
using MiniCommerce.OrderService.Models;
using MiniCommerce.Shared.Brokers.Events;
using MiniCommerce.Shared.Events;
using static MiniCommerce.OrderService.Models.DTOs.OrderDtos;

namespace MiniCommerce.OrderService.Services.Foundations;
public class OrderService(IStorageBroker storageBroker, IEventPublisher eventPublisher, IHttpClientFactory httpClientFactory) : IOrderService
{
    public async ValueTask AddOrderAsync(Order order)
    {
        if (order.Quantity <= 0)
            throw new InvalidQuantityException();

        var client = httpClientFactory.CreateClient("ProductClient");
        var response = await client.GetAsync($"/api/products/{order.ProductId}");
        
        if (!response.IsSuccessStatusCode)
            throw new Exception("Product not found or Product Service is currently down!");

        var product = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        if (product is null)
            throw new Exception("Failed to read product data.");

        decimal secureTotalPrice = product.Price * order.Quantity;

        var userClient = httpClientFactory.CreateClient("UserClient");
        var userResponse = await userClient.GetAsync($"/api/users/{order.UserId}");

        if (!userResponse.IsSuccessStatusCode)
            throw new Exception("User not found or User Service is down!");

        var user = await userResponse.Content.ReadFromJsonAsync<UserResponseDto>();
        if (user is null)
            throw new Exception("Failed to read user data.");

        if (user.WalletBalance < secureTotalPrice)
            throw new Exception($"Insufficient funds. Order total is {secureTotalPrice}, but wallet only has {user.WalletBalance}.");
        
        order.Status = OrderStatus.Pending; 
        await storageBroker.InsertOrderAsync(order);

        var orderEvent = new OrderCreatedEvent(
           order.Id,
           order.UserId,
           order.ProductId,
           order.Quantity,
           secureTotalPrice
       );

        await eventPublisher.PublishAsync("order-events", orderEvent);
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
