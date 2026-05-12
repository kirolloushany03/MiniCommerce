namespace MiniCommerce.ProductService.Models.DTOs;

public class ProductDtos
{
    public record CreateProductDto(string Name, decimal Price, int InitialStock);
    public record UpdateProductDto(string Name, decimal Price);
    public record UpdateStockDto(int Quantity);

}
