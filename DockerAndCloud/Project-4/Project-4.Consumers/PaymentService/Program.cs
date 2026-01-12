// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using CoreLibrary.Helper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CoreLibrary.Models;

Console.WriteLine("Hello, World!");

var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST")
               ?? throw new InvalidOperationException("RABBITMQ_HOST environment variable is not set");

var factory = new ConnectionFactory
{
    HostName = hostName,
    UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest",
    Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest",
};

IConnection connection = null;
Console.WriteLine("Connecting to RabbitMQ...");
while (connection == null)
{
    try
    {
        Console.WriteLine("Connecting to RabbitMQ...");
        connection = await factory.CreateConnectionAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"RabbitMQ not ready: {ex.Message}");
        await Task.Delay(5000);
    }
}
var channel = await connection.CreateChannelAsync();


var exchangeName = "direct";
var exchangeName2 = "topic";
var exchangeName3 = "project4.all.exchange";

await channel.ExchangeDeclareAsync(exchangeName, "direct", true, false);
await channel.ExchangeDeclareAsync(exchangeName2, "topic", true, false);
await channel.ExchangeDeclareAsync(exchangeName3, "fanout", true, false);


var queueName = "payment.queue";
await channel.QueueDeclareAsync(queueName, true, false, false);

var routingKey = "payment";
var routingKey2 = "payment.#";
var routingKey3 = "*.*.payment";
var routingKey4 = "efgh";

await channel.QueueBindAsync(queueName, exchangeName, routingKey);
await channel.QueueBindAsync(queueName, exchangeName2, routingKey2);
await channel.QueueBindAsync(queueName, exchangeName2, routingKey3);
await channel.QueueBindAsync(queueName, exchangeName3, routingKey4);


Console.WriteLine($"Queue bound: queue: {queueName}, exchange: {exchangeName}, routingKey: {routingKey}");
var consumer = new AsyncEventingBasicConsumer(channel);

var responseExchangeName = RabbitmqConstants.ResponseExchangeName;

consumer.ReceivedAsync += PaymentServiceConsumer;
Console.WriteLine("Exchange and queue declared!");

await channel.ExchangeDeclareAsync(responseExchangeName, "direct", true, false);

async Task PaymentServiceConsumer(object sender, BasicDeliverEventArgs @event)
{
    Console.WriteLine($"At PaymentService...");
    var body = @event.Body;
    var msgBody = Encoding.UTF8.GetString(body.ToArray());
    Console.WriteLine("Received msg: " + msgBody);
    
    await Task.Delay(1000);
    await channel.BasicAckAsync(@event.DeliveryTag, false);
    var msgEnvelope = JsonSerializer.Deserialize<MessageEnvelop<string>>(msgBody);
    await RabbitmqHelper.PublishResponseAsync(msgEnvelope, @event, "Payment", channel);
}

await channel.BasicConsumeAsync(queueName, false, consumer);
Console.WriteLine("Press [enter] to exit");
await Task.Delay(Timeout.Infinite);