using Confluent.Kafka;
using MiniCommerce.ProductService.Services.Foundations;
using MiniCommerce.Shared.Events;
using System.Text.Json;

namespace MiniCommerce.ProductService.Services.Hosted;

public class OrderEventConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory, ILogger<OrderEventConsumer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";

        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "product-service-group", 
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Null, string>(config).Build();

        consumer.Subscribe("order-events");

        logger.LogInformation(" Product Service is now listening to 'order-events'...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);
                var messageJson = consumeResult.Message.Value;

                var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(messageJson);

                if (orderEvent is not null)
                {
                    logger.LogInformation($"📦 Received Order {orderEvent.OrderId}. Deducting {orderEvent.Quantity} from Product {orderEvent.ProductId}...");

                    using var scope = scopeFactory.CreateScope();
                    var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

                    await productService.UpdateStockAsync(orderEvent.ProductId, -orderEvent.Quantity);

                    logger.LogInformation("✅ Stock deducted successfully.");
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError($"❌ Error processing Kafka message: {ex.Message}");
            }
        }

        consumer.Close();
    }
}
