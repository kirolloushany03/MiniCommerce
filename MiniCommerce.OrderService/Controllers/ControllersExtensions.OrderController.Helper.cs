using Microsoft.AspNetCore.Mvc;
using MiniCommerce.OrderService.Models;
using MiniCommerce.OrderService.Services.Foundations;
using static MiniCommerce.OrderService.Models.DTOs.OrderDtos;

namespace MiniCommerce.OrderService.Controllers;

public static partial class ControllersExtensions
{
    static async ValueTask<IResult> PostOrderAsync(IOrderService orderService, [FromBody] CreateOrderDto dto)
    {
        try
        {
            var order = new Order
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await orderService.AddOrderAsync(order);
            return Results.Created();
        }
        catch (InvalidQuantityException ex)
        {
            return Results.BadRequest(new { ex.Message });
        }
    }

    static async ValueTask<IResult> GetAllOrdersAsync(IOrderService orderService) =>
        Results.Ok(await orderService.RetrieveAllOrdersAsync());

    static async ValueTask<IResult> GetOrderByIdAsync(Guid id, IOrderService orderService)
    {
        var order = await orderService.RetrieveOrderByIdAsync(id);
        return order is not null ? Results.Ok(order) : Results.NotFound(new { Message = "Order not found." });
    }

    static async ValueTask<IResult> DeleteOrderAsync(Guid id, IOrderService orderService)
    {
        try
        {
            await orderService.RemoveOrderByIdAsync(id);
            return Results.NoContent();
        }
        catch (OrderNotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }
    }
}
