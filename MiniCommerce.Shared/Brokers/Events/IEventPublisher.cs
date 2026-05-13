namespace MiniCommerce.Shared.Brokers.Events;

public  interface IEventPublisher
{
    Task PublishAsync<T>(string topic, T @event);
}
