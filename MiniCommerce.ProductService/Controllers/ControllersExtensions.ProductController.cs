namespace MiniCommerce.ProductService.Controllers;

public partial class ControllersExtensions
{
    public static WebApplication MapProductEndpoints(this WebApplication app)
    {
        var groupName = "Products";
        var group = app.MapGroup("/api/products").WithTags(groupName);

        group.MapPost("", PostProductAsync)
            .WithTags(groupName)
            .WithSummary(nameof(PostProductAsync))
            .WithDescription("Create a new product");

        group.MapGet("", GetAllProductsAsync)
            .WithTags(groupName)
            .WithSummary(nameof(GetAllProductsAsync))
            .WithDescription("Get all products");

        group.MapGet("/{id:guid}", GetProductByIdAsync)
            .WithTags(groupName)
            .WithSummary(nameof(GetProductByIdAsync))
            .WithDescription("Get product by ID");

        group.MapPut("/{id:guid}", PutProductAsync)
            .WithTags(groupName)
            .WithSummary(nameof(PutProductAsync))
            .WithDescription("Update product details");

        group.MapDelete("/{id:guid}", DeleteProductAsync)
            .WithTags(groupName)
            .WithSummary(nameof(DeleteProductAsync))
            .WithDescription("Delete a product");

        group.MapPost("/{id:guid}/update-stock", UpdateStockAsync)
            .WithTags(groupName)
            .WithSummary(nameof(UpdateStockAsync))
            .WithDescription("Update product stock");

        return app;
    }
}
