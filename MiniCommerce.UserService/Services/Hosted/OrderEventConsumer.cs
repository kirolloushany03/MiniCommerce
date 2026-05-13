using Confluent.Kafka;
using MiniCommerce.UserService.Services.Foundations;
using MiniCommerce.Shared.Events;
using System.Text.Json;

namespace MiniCommerce.UserService.Services.Hosted;

public class OrderEventConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory, ILogger<OrderEventConsumer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
            GroupId = "user-service-group", 
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Null, string>(config).Build();
        consumer.Subscribe("order-events");

        logger.LogInformation(" User Service is now listening to 'order-events'...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken); 
                var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(consumeResult.Message.Value);

                if (orderEvent is not null)
                {
                    logger.LogInformation($" Received Order {orderEvent.OrderId}. Deducting {orderEvent.TotalPrice} from User {orderEvent.UserId}...");

                    using var scope = scopeFactory.CreateScope();
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                    await userService.DeductBalanceAsync(orderEvent.UserId, orderEvent.TotalPrice);

                    logger.LogInformation(" Balance deducted successfully.");
                }
            }
            catch (OperationCanceledException)
            {
                break; 
            }
            catch (Exception ex)
            {
                logger.LogError($" Error processing Kafka message: {ex.Message}");
            }
        }

        consumer.Close();
    }
}
