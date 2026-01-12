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

var queueName = "email.queue";
await channel.QueueDeclareAsync(queueName, true, false, false);

var routingKey = "email";
var routingKey2 = "email.*";
var routingKey3 = "*.email.*";
var routingKey4 = "ABCD";
await channel.QueueBindAsync(queueName, exchangeName, routingKey);
await channel.QueueBindAsync(queueName, exchangeName2, routingKey2);
await channel.QueueBindAsync(queueName, exchangeName2, routingKey3);
await channel.QueueBindAsync(queueName, exchangeName3, routingKey4);


Console.WriteLine($"Queue bound: queue: {queueName}, exchange: {exchangeName}, routingKey: {routingKey}");
var consumer = new AsyncEventingBasicConsumer(channel);

var responseExchangeName = RabbitmqConstants.ResponseExchangeName;
var responseExchangeRoutingKey = RabbitmqConstants.ResponseExchangeRoutingKey;

consumer.ReceivedAsync += EmailServiceConsumer;
Console.WriteLine("Exchange and queue declared!");

await channel.ExchangeDeclareAsync(responseExchangeName, "direct", true, false);

async Task EmailServiceConsumer(object sender, BasicDeliverEventArgs @event)
{
    Console.WriteLine($"At EmailService...");
    var body = @event.Body;
    var msgBody = Encoding.UTF8.GetString(body.ToArray());
    Console.WriteLine($"Received Message: {msgBody}");
    await Task.Delay(1000);
    await channel.BasicAckAsync(@event.DeliveryTag, false);
    var msgEnvelope = JsonSerializer.Deserialize<MessageEnvelop<string>>(msgBody);
    await RabbitmqHelper.PublishResponseAsync(msgEnvelope, @event, "EmailService", channel);
}

await channel.BasicConsumeAsync(queueName, false, consumer);

Console.WriteLine("Press [enter] to exit");
await Task.Delay(Timeout.Infinite);
