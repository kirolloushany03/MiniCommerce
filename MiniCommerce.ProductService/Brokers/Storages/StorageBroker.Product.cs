using Microsoft.EntityFrameworkCore;
using MiniCommerce.ProductService.Models;

namespace MiniCommerce.ProductService.Brokers.Storages;

public partial class StorageBroker
{
    public async ValueTask InsertProductAsync(Product product)
    {
        await this.Products.AddAsync(product);
        await this.SaveChangesAsync();
    }

    public async ValueTask<IQueryable<Product>> SelectAllProductsAsync() =>
        await ValueTask.FromResult(this.Products.AsQueryable());

    public async ValueTask<Product?> SelectProductByIdAsync(Guid id) =>
        await this.Products.FindAsync(id);

    public async ValueTask<Product?> SelectProductByNameAsync(string name) =>
        await this.Products.FirstOrDefaultAsync(p => p.Name == name);

    public async ValueTask UpdateProductAsync(Product product)
    {
        this.Products.Update(product);
        await this.SaveChangesAsync();
    }

    public async ValueTask DeleteProductAsync(Product product)
    {
        this.Products.Remove(product);
        await this.SaveChangesAsync();
    }
}
