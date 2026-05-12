namespace MiniCommerce.OrderService.Models;

public class OrderNotFoundException() : Exception("Order not found.");
public class InvalidQuantityException() : Exception("Quantity must be greater than zero.");