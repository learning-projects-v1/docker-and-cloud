using RabbitMQ.Client;

namespace CoreLibrary.Services;

public interface IRabbitmqTopologyService
{
    Task DeclareAndBindAsync(
        string exchangeName,
        string queueName,
        string routingKey,
        string exchangeType,
        IChannel channel,
        CancellationToken cancellationToken = default);
}

public class RabbitmqTopologyService : IRabbitmqTopologyService
{
    public async Task DeclareAndBindAsync(string exchangeName, string queueName, string routingKey,string exchangeType, IChannel channel,
        CancellationToken cancellationToken = default)
    {
        await channel.ExchangeDeclareAsync(exchangeName, exchangeType, true, false, cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(queueName, true, false, false, cancellationToken: cancellationToken);
        await channel.QueueBindAsync(queueName, exchangeName, routingKey, cancellationToken: cancellationToken);
    }
}