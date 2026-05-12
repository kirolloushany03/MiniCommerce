using MiniCommerce.UserService.Models;

namespace MiniCommerce.UserService.Brokers.Storages;
public partial interface IStorageBroker
{
    ValueTask InsertUserAsync(User user);
    ValueTask<IQueryable<User>> SelectAllUsersAsync();
    ValueTask<User?> SelectUserByIdAsync(Guid id);
    ValueTask<User?> SelectUserByEmailAsync(string email);
    ValueTask UpdateUserAsync(User user);
    ValueTask DeleteUserAsync(User user);
}
