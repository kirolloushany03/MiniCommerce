using Microsoft.EntityFrameworkCore;
using MiniCommerce.ProductService.Services.Hosted;
using MiniCommerce.UserService.Brokers.Storages;
using MiniCommerce.UserService.Controllers;
using MiniCommerce.UserService.Services.Foundations;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


// 1. Database Setup (SQLite)
builder.Services.AddDbContext<StorageBroker>(options =>
    options.UseSqlite("Data Source=Database/users.db"));

// 2. Dependency Injection (DI)
builder.Services.AddScoped<IStorageBroker, StorageBroker>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHostedService<OrderEventConsumer>();


// 3. OpenAPI Documentation Setup (New in .NET 9/10)
builder.Services.AddOpenApi();

var app = builder.Build();

// 4. Auto-Create Database on Startup (Magic! ✨)
using (var scope = app.Services.CreateScope())
{
    var broker = scope.ServiceProvider.GetRequiredService<StorageBroker>();
    broker.Database.EnsureCreated();
}

app.MapOpenApi();
app.MapScalarApiReference();

app.MapUserEndpoints();


app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    var urls = app.Urls;

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\n=========================================");
    Console.WriteLine(" MICROSERVICE STARTED SUCCESSFULLY ");
    Console.WriteLine("=========================================\n");

    foreach (var url in urls)
    {
        Console.WriteLine($" Scalar UI: {url}/scalar/v1");
        Console.WriteLine($" OpenAPI JSON: {url}/openapi/v1.json\n");
    }
    Console.ResetColor();
});

app.Run();