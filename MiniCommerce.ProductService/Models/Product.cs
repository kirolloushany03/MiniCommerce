using Microsoft.EntityFrameworkCore;

namespace MiniCommerce.ProductService.Models;

[Index(nameof(Name), IsUnique = true)]
public class Product
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}
