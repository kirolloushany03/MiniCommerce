using Microsoft.EntityFrameworkCore;
using MiniCommerce.OrderService.Models;

namespace MiniCommerce.OrderService.Brokers.Storages;

public partial class StorageBroker(DbContextOptions<StorageBroker> options) : DbContext(options), IStorageBroker
{
    public DbSet<Order> Orders { get; set; }
}
