using Microsoft.EntityFrameworkCore;
using MiniCommerce.UserService.Models;

namespace MiniCommerce.UserService.Brokers.Storages;

public partial class StorageBroker(DbContextOptions<StorageBroker> options) : DbContext(options), IStorageBroker
{
        public DbSet<Models.User> Users { get; set; }
}
