using Microsoft.EntityFrameworkCore;
using MiniCommerce.UserService.Brokers.Storages;
using MiniCommerce.UserService.Models;

namespace MiniCommerce.UserService.Services.Foundations;

public class UserService(IStorageBroker storageBroker) : IUserService
{
    public async ValueTask AddUserAsync(User user)
    {
        var existingUser = await storageBroker.SelectUserByEmailAsync(user.Email);
        if (existingUser is not null)
            throw new EmailAlreadyInUseException();

        await storageBroker.InsertUserAsync(user);
    }

    public async ValueTask<IEnumerable<User>> RetrieveAllUsersAsync() 
    {
        var usersQuery = await storageBroker.SelectAllUsersAsync();
        return await usersQuery.ToListAsync();
    }

    public async ValueTask<User?> RetrieveUserByIdAsync(Guid id) =>
       await storageBroker.SelectUserByIdAsync(id);

    public async ValueTask<User?> RetrieveUserByEmailAsync(string email) =>
        await storageBroker.SelectUserByEmailAsync(email);

    public async ValueTask ModifyUserAsync(User user)
    {
        var existingUser = await storageBroker.SelectUserByIdAsync(user.Id);
        if (existingUser is null)
            throw new UserNotFoundException();

        var emailCheck = await storageBroker.SelectUserByEmailAsync(user.Email);
        if (emailCheck is not null && emailCheck.Id != user.Id)
            throw new EmailAlreadyInUseException();

        await storageBroker.UpdateUserAsync(user);
    }
    public async ValueTask RemoveUserByIdAsync(Guid id)
    {
        var user = await storageBroker.SelectUserByIdAsync(id);
        if (user is null)
            throw new UserNotFoundException();

        await storageBroker.DeleteUserAsync(user);
    }

    public async ValueTask AddBalanceAsync(Guid id, decimal amount)
    {
        if (amount <= 0)
            throw new InvalidAmountException();

        var user = await storageBroker.SelectUserByIdAsync(id);
        if (user is null)
            throw new UserNotFoundException();

        user.WalletBalance += amount;
        await storageBroker.UpdateUserAsync(user);
    }

    public async ValueTask DeductBalanceAsync(Guid id, decimal amount)
    {
        if (amount <= 0)
            throw new InvalidAmountException();

        var user = await storageBroker.SelectUserByIdAsync(id);
        if (user is null)
            throw new UserNotFoundException();

        if (user.WalletBalance < amount)
            throw new InsufficientFundsException();

        user.WalletBalance -= amount;
        await storageBroker.UpdateUserAsync(user);
    }

}
