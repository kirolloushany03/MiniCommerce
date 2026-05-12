using MiniCommerce.ProductService.Models;

namespace MiniCommerce.ProductService.Services.Foundations;
public interface IProductService
{
    ValueTask AddProductAsync(Product product);
    ValueTask<IEnumerable<Product>> RetrieveAllProductsAsync();
    ValueTask<Product?> RetrieveProductByIdAsync(Guid id);
    ValueTask ModifyProductAsync(Product product);
    ValueTask RemoveProductByIdAsync(Guid id);
    ValueTask UpdateStockAsync(Guid id, int quantity);

}
