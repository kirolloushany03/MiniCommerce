using MiniCommerce.ProductService.Models;

namespace MiniCommerce.ProductService.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask InsertProductAsync(Product product);
    ValueTask<IQueryable<Product>> SelectAllProductsAsync();
    ValueTask<Product?> SelectProductByIdAsync(Guid id);
    ValueTask<Product?> SelectProductByNameAsync(string name);
    ValueTask UpdateProductAsync(Product product);
    ValueTask DeleteProductAsync(Product product);
}
