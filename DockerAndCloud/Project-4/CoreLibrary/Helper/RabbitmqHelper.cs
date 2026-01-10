using System.Text;
using System.Text.Json;
using CoreLibrary.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CoreLibrary.Helper;


public static class RabbitmqHelper
{
    private static readonly string ResponseExchangeName = RabbitmqConstants.ResponseExchangeName;
    private static readonly string ResponseExchangeRoutingKey = RabbitmqConstants.ResponseExchangeRoutingKey;
    
    public static  async Task PublishResponseAsync(MessageEnvelop<string> msgEnvelope, BasicDeliverEventArgs @event, string serviceName, IChannel channel)
    {
        var responseMessagee = $"{msgEnvelope.Payload} processed. @{serviceName}";
        var responseMessageEnvelope = new MessageEnvelop<string>
        {
            Payload = responseMessagee,
            CorrelationId = msgEnvelope.CorrelationId
        };
    
        var responseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(responseMessageEnvelope));
        Console.WriteLine($"Publishing response to ExchangeName: @{ResponseExchangeName}, routingKey: @{ResponseExchangeRoutingKey}");
        await channel.BasicPublishAsync(ResponseExchangeName, ResponseExchangeRoutingKey, responseBytes);
    }
}