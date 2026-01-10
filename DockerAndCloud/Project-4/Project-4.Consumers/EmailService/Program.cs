// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using CoreLibrary.Helper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CoreLibrary.Models;

Console.WriteLine("Hello, World!");

var factory = new ConnectionFactory() { HostName = "localhost" };
var connection = await factory.CreateConnectionAsync();
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
Console.ReadLine();