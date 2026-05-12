namespace MiniCommerce.OrderService.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}
