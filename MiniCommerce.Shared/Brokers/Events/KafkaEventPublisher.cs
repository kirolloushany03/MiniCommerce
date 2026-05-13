using Confluent.Kafka;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace MiniCommerce.Shared.Brokers.Events;

public class KafkaEventPublisher : IEventPublisher
{
    private readonly IProducer<Null, string> _producer;

    public KafkaEventPublisher(IConfiguration configuration)
    {
        var bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";

        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishAsync<T>(string topic, T @event)
    {
        var message = new Message<Null, string>
        {
            Value = JsonSerializer.Serialize(@event)
        };

        await _producer.ProduceAsync(topic, message);
    }

}
