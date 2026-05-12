using Microsoft.EntityFrameworkCore;
using MiniCommerce.ProductService.Brokers.Storages;
using MiniCommerce.ProductService.Models;

namespace MiniCommerce.ProductService.Services.Foundations;

public class ProductService(IStorageBroker storageBroker) : IProductService
{
    public async ValueTask AddProductAsync(Product product)
    {
        var existingProduct = await storageBroker.SelectProductByNameAsync(product.Name);
        if (existingProduct is not null)
            throw new ProductNameAlreadyExistsException();

        await storageBroker.InsertProductAsync(product);
    }

    public async ValueTask<IEnumerable<Product>> RetrieveAllProductsAsync()
    {
        var productsQuery = await storageBroker.SelectAllProductsAsync();
        return await productsQuery.ToListAsync();
    }

    public async ValueTask<Product?> RetrieveProductByIdAsync(Guid id) =>
        await storageBroker.SelectProductByIdAsync(id);

    public async ValueTask ModifyProductAsync(Product product)
    {
        var existingProduct = await storageBroker.SelectProductByIdAsync(product.Id);
        if (existingProduct is null)
            throw new ProductNotFoundException();

        var nameCheck = await storageBroker.SelectProductByNameAsync(product.Name);
        if (nameCheck is not null && nameCheck.Id != product.Id)
            throw new ProductNameAlreadyExistsException();

        await storageBroker.UpdateProductAsync(product);
    }

    public async ValueTask RemoveProductByIdAsync(Guid id)
    {
        var product = await storageBroker.SelectProductByIdAsync(id);
        if (product is null)
            throw new ProductNotFoundException();

        await storageBroker.DeleteProductAsync(product);
    }

    public async ValueTask UpdateStockAsync(Guid id, int quantity)
    {
        var product = await storageBroker.SelectProductByIdAsync(id);
        if (product is null)
            throw new ProductNotFoundException();

        if (product.StockQuantity + quantity < 0)
            throw new InvalidStockUpdateException();

        product.StockQuantity += quantity;
        await storageBroker.UpdateProductAsync(product);
    }
}
