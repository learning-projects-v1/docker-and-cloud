// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using CoreLibrary.Helper;
using CoreLibrary.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello, World!");
const string unavailableServiceName = "'Highly Unavailable Service'";
var factory = new ConnectionFactory() { HostName = "localhost" };
var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

var unavailableServiceExchange = "project4.unavailable.exchange";
var retryExchange = "project4.retry.exchange";
var dlxExchange = "project4.dlx.exchange";
var fanoutExchange = "project4.all.exchange";

await channel.ExchangeDeclareAsync(unavailableServiceExchange, ExchangeType.Topic, true, false);
await channel.ExchangeDeclareAsync(retryExchange, ExchangeType.Topic, true, false);
await channel.ExchangeDeclareAsync(dlxExchange, ExchangeType.Topic, true, false);
await channel.ExchangeDeclareAsync(fanoutExchange, ExchangeType.Fanout, true, false);

var unavailableQueue = "project4.unavailable.queue";
var retryQueue = "project4.retry.queue";
var dlqQueue = "project4.dlq.queue";
var unavailableRoutingKey = "unavailable";
var retryRoutingKey = "retry";

var unavailableQueueArgs = new Dictionary<string, object>
{
    { "x-dead-letter-exchange", retryExchange },
    { "x-dead-letter-routing-key", retryRoutingKey },
};

var retryQueueArgs = new Dictionary<string, object>
{
    { "x-message-ttl", 5000 },
    { "x-dead-letter-exchange", unavailableServiceExchange },
    { "x-dead-letter-routing-key", unavailableRoutingKey },
};

await channel.QueueDeclareAsync(unavailableQueue, true, false, false, unavailableQueueArgs!);
await channel.QueueDeclareAsync(retryQueue, true, false, false, retryQueueArgs!);
await channel.QueueDeclareAsync(dlqQueue, true, false);

await channel.QueueBindAsync(unavailableQueue, unavailableServiceExchange, unavailableRoutingKey);
await channel.QueueBindAsync(retryQueue, retryExchange, retryRoutingKey);
await channel.QueueBindAsync(dlqQueue, dlxExchange, "");
await channel.QueueBindAsync(unavailableQueue, fanoutExchange, "dfe");


Console.WriteLine("Queue binding done");
var unavailableConsumer = new AsyncEventingBasicConsumer(channel);
const int maxRetryCount = 3;
var rnd = new Random();
unavailableConsumer.ReceivedAsync += async (s, e) =>
{
    
    var tp = Encoding.UTF8.GetString(e.Body.ToArray());
    var tpEnvelope = JsonSerializer.Deserialize<MessageEnvelop<string>>(tp);
    var headers = e.BasicProperties.Headers;
    if (headers != null && headers.TryGetValue("x-death", out var death))
    {
        
        var deathList = (IList<object>)death!;
        if (deathList?.Count > 0)
        {
            var deathEntry = (IDictionary<string, object>)deathList.First();
            if (deathEntry.TryGetValue("count", out var value))
            {
                var retryCount = (long)value;
                if (retryCount >= maxRetryCount)
                {
                    await channel.BasicPublishAsync(dlxExchange, "", false, e.Body);
                    await channel.BasicAckAsync(e.DeliveryTag, false);
                    Console.WriteLine($"{tpEnvelope.CorrelationId} :: Publishing to dlq");
                    tpEnvelope.Payload = $"FAILED!! Pushed to DLQ!";
                    await RabbitmqHelper.PublishResponseAsync(tpEnvelope, e, unavailableServiceName, channel);
                    return;
                }

                var retryMsg = $"RETRY:: retry count: {retryCount}";
                tpEnvelope.Payload = retryMsg;
                
                await RabbitmqHelper.PublishResponseAsync(tpEnvelope, e, unavailableServiceName, channel);
                Console.WriteLine($"{tpEnvelope.CorrelationId} :: sent for retry :: RetryCount: " + retryCount);
            }
        }
    }


    var prob = rnd.NextInt64(10);
    Console.WriteLine($"ID: {tpEnvelope.CorrelationId} ::  Probability value: {prob}.");
    if (prob < 7)
    {
        await channel.BasicNackAsync(e.DeliveryTag, false, false);
        return;
    }
    var msg = Encoding.UTF8.GetString(e.Body.ToArray());
    await channel.BasicAckAsync(e.DeliveryTag, false);
    var msgEnvelope = JsonSerializer.Deserialize<MessageEnvelop<string>>(msg);
    Console.WriteLine($"At consumer @{unavailableQueue}. Message id: {msgEnvelope.CorrelationId} successfully received!");
    
    await RabbitmqHelper.PublishResponseAsync(msgEnvelope, e, unavailableServiceName, channel);
};
await channel.BasicConsumeAsync(unavailableQueue, false, unavailableConsumer);


// update - no one should listen on retry queue
// var retryConsumer = new AsyncEventingBasicConsumer(channel);
// retryConsumer.ReceivedAsync += async (s, e) =>
// {
//     Console.WriteLine($"At {retryQueue} Consumer. Message received");
//     var msg = Encoding.UTF8.GetString( e.Body.ToArray());
//     Console.WriteLine($"Msg: {msg}");
//     
// };
// await channel.BasicConsumeAsync(retryQueue, false, retryConsumer);

Console.WriteLine("Press [enter] to exit");
Console.ReadLine();