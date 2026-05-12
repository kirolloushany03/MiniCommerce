namespace MiniCommerce.OrderService.Controllers;

public static partial class ControllersExtensions
{
    public static WebApplication MapOrderEndpoints(this WebApplication app)
    {
        var groupName = "Orders";
        var group = app.MapGroup("/api/orders")
            .WithTags(groupName);

        group.MapPost("", PostOrderAsync)
            .WithTags(groupName)
            .WithSummary(nameof(PostOrderAsync))
            .WithDescription("Create a new order");

        group.MapGet("", GetAllOrdersAsync)
            .WithTags(groupName)
            .WithSummary(nameof(GetAllOrdersAsync))
            .WithDescription("Get all orders");

        group.MapGet("/{id:guid}", GetOrderByIdAsync)
            .WithTags(groupName)
            .WithSummary(nameof(GetOrderByIdAsync))
            .WithDescription("Get order by ID");

        group.MapDelete("/{id:guid}", DeleteOrderAsync)
            .WithTags(groupName)
            .WithSummary(nameof(DeleteOrderAsync))
            .WithDescription("Delete an order");

        return app;
    }
}
