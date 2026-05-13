namespace MiniCommerce.OrderService.Models.DTOs;
public class OrderDtos
{
    public record CreateOrderDto(Guid UserId, Guid ProductId, int Quantity);
    public record ProductResponseDto(decimal Price);
    public record UserResponseDto(decimal WalletBalance);

}
