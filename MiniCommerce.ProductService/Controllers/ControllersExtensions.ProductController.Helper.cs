using Microsoft.AspNetCore.Mvc;
using MiniCommerce.ProductService.Models;
using MiniCommerce.ProductService.Services.Foundations;
using static MiniCommerce.ProductService.Models.DTOs.ProductDtos;

namespace MiniCommerce.ProductService.Controllers;

public static partial class ControllersExtensions
{
    static async ValueTask<IResult> PostProductAsync(IProductService productService, [FromBody] CreateProductDto dto)
    {
        try
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                StockQuantity = dto.InitialStock
            };

            await productService.AddProductAsync(product);
            return Results.Created();
        }
        catch (ProductNameAlreadyExistsException ex)
        {
            return Results.Conflict(new { ex.Message });
        }
    }

    static async ValueTask<IResult> GetAllProductsAsync(IProductService productService) =>
        Results.Ok(await productService.RetrieveAllProductsAsync());

    static async ValueTask<IResult> GetProductByIdAsync(Guid id, IProductService productService)
    {
        var product = await productService.RetrieveProductByIdAsync(id);
        return product is not null ? Results.Ok(product) : Results.NotFound(new { Message = "Product not found." });
    }

    static async ValueTask<IResult> PutProductAsync(Guid id, IProductService productService, [FromBody] UpdateProductDto dto)
    {
        try
        {
            var existingProduct = await productService.RetrieveProductByIdAsync(id);
            if (existingProduct is null) return Results.NotFound(new { Message = "Product not found." });

            existingProduct.Name = dto.Name;
            existingProduct.Price = dto.Price;

            await productService.ModifyProductAsync(existingProduct);
            return Results.Ok(existingProduct);
        }
        catch (ProductNotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }
        catch (ProductNameAlreadyExistsException ex)
        {
            return Results.Conflict(new { ex.Message });
        }
    }

    static async ValueTask<IResult> DeleteProductAsync(Guid id, IProductService productService)
    {
        try
        {
            await productService.RemoveProductByIdAsync(id);
            return Results.NoContent();
        }
        catch (ProductNotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }
    }

    static async ValueTask<IResult> UpdateStockAsync(Guid id, [FromBody] UpdateStockDto dto, IProductService productService)
    {
        try
        {
            await productService.UpdateStockAsync(id, dto.Quantity);
            return Results.Ok(new { Message = "Stock updated successfully." });
        }
        catch (ProductNotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }
        catch (InvalidStockUpdateException ex)
        {
            return Results.BadRequest(new { ex.Message });
        }
    }
}
