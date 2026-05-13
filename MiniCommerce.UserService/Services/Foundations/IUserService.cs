using MiniCommerce.UserService.Models;

namespace MiniCommerce.UserService.Services.Foundations;
public interface IUserService
{
        ValueTask AddUserAsync(User user);
        ValueTask<IEnumerable<User>> RetrieveAllUsersAsync();
        ValueTask<User?> RetrieveUserByIdAsync(Guid id);
        ValueTask<User?> RetrieveUserByEmailAsync(string email);
        ValueTask ModifyUserAsync(User user);
        ValueTask RemoveUserByIdAsync(Guid id);

        ValueTask AddBalanceAsync(Guid id, decimal amount);
         ValueTask DeductBalanceAsync(Guid id, decimal amount);

}
