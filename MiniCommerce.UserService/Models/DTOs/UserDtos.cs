namespace MiniCommerce.UserService.Models.DTOs;
public class UserDtos
{
    public record CreateUserDto(string Name, string Email, decimal InitialBalance);
    public record UpdateUserDto(string Name, string Email);
    public record AddBalanceDto(decimal Amount);

}
