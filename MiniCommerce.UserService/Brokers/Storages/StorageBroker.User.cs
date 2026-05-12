using Microsoft.EntityFrameworkCore;
using MiniCommerce.UserService.Models;
using System.Runtime.InteropServices;

namespace MiniCommerce.UserService.Brokers.Storages;
public partial class StorageBroker
{
    public async ValueTask InsertUserAsync(User user)
    {
        await this.AddAsync(user);
        await this .SaveChangesAsync();
    }

    public async ValueTask<IQueryable<User>> SelectAllUsersAsync() =>
        await ValueTask.FromResult(this.Users.AsQueryable());

    public async ValueTask<User?> SelectUserByIdAsync(Guid id) =>
        await this.Users.FindAsync(id);

    public async ValueTask<User?> SelectUserByEmailAsync(string email) =>
        await this.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async ValueTask UpdateUserAsync(User user)
    {
        this.Users.Update(user);
        await this.SaveChangesAsync();
    }

    public async ValueTask DeleteUserAsync(User user)
    {
        this.Users.Remove(user);
        await this.SaveChangesAsync();
    }
     
}
