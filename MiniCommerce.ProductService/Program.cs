using Microsoft.EntityFrameworkCore;
using MiniCommerce.ProductService.Brokers.Storages;
using MiniCommerce.ProductService.Controllers;
using MiniCommerce.ProductService.Services.Foundations;
using Scalar.AspNetCore;
using MiniCommerce.ProductService.Services.Hosted;


var builder = WebApplication.CreateBuilder(args);

// 1. Database Setup
builder.Services.AddDbContext<StorageBroker>(options =>
    options.UseSqlite("Data Source=Database/products.db"));

// 2. Dependency Injection
builder.Services.AddScoped<IStorageBroker, StorageBroker>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddHostedService<OrderEventConsumer>();


// 3. OpenAPI Docs
builder.Services.AddOpenApi();

var app = builder.Build();

// 4. Auto-Create Database
using (var scope = app.Services.CreateScope())
{
    var broker = scope.ServiceProvider.GetRequiredService<StorageBroker>();
    broker.Database.EnsureCreated();
}

app.MapOpenApi();
app.MapScalarApiReference();

// 5. Map Endpoints
app.MapProductEndpoints();

// 6. Console Magic
app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    var urls = app.Urls;

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n=========================================");
    Console.WriteLine(" PRODUCT MICROSERVICE STARTED ");
    Console.WriteLine("=========================================\n");

    foreach (var url in urls)
    {
        Console.WriteLine($" Scalar UI: {url}/scalar/v1");
        Console.WriteLine($" OpenAPI JSON: {url}/openapi/v1.json\n");
    }
    Console.ResetColor();
});

app.Run();