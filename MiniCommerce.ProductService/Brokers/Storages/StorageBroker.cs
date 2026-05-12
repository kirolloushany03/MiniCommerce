using Microsoft.EntityFrameworkCore;
using MiniCommerce.ProductService.Models;

namespace MiniCommerce.ProductService.Brokers.Storages;

public partial class StorageBroker(DbContextOptions<StorageBroker> options) : DbContext(options), IStorageBroker
{
    public DbSet<Product> Products { get; set; }
}
