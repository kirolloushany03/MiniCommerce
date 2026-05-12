namespace MiniCommerce.ProductService.Models;

public class ProductNotFoundException()
    : Exception("Product not found.");

public class ProductNameAlreadyExistsException()
    : Exception("A product with this name already exists.");

public class InvalidStockUpdateException()
    : Exception("Invalid stock update. Stock cannot be negative.");
