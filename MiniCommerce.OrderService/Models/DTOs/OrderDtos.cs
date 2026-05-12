namespace MiniCommerce.OrderService.Models.DTOs;
public class OrderDtos
{
    public record CreateOrderDto(Guid UserId, Guid ProductId, int Quantity);
}
