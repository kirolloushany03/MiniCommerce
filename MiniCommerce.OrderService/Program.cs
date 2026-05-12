using Microsoft.EntityFrameworkCore;
using MiniCommerce.OrderService.Brokers.Storages;
using MiniCommerce.OrderService.Controllers;
using MiniCommerce.OrderService.Services.Foundations;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StorageBroker>(options =>
    options.UseSqlite("Data Source=orders.db"));

builder.Services.AddScoped<IStorageBroker, StorageBroker>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var broker = scope.ServiceProvider.GetRequiredService<StorageBroker>();
    broker.Database.EnsureCreated();
}

app.MapOpenApi();
app.MapScalarApiReference();

app.MapOrderEndpoints();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    var urls = app.Urls;

    Console.ForegroundColor = ConsoleColor.Yellow; 
    Console.WriteLine("\n=========================================");
    Console.WriteLine(" ORDER MICROSERVICE STARTED");
    Console.WriteLine("=========================================\n");

    foreach (var url in urls)
    {
        Console.WriteLine($" Scalar UI: {url}/scalar/v1");
        Console.WriteLine($" OpenAPI JSON: {url}/openapi/v1.json\n");
    }
    Console.ResetColor();
});

app.Run();