using System.Text;
using System.Text.Json;
using CoreLibrary.Models;
using Microsoft.AspNetCore.SignalR;
using Project_4.Publisher.Api.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Project_4.Publisher.Api.Services;

public class RabbitmqConsumerService : BackgroundService
{
    private readonly IMessageRepository<RabbitmqResponse> _messageRepository;
    private readonly IHubContext<NotificaitonHub> _hubContext;
    public RabbitmqConsumerService(IMessageRepository<RabbitmqResponse> messageRepository, IHubContext<NotificaitonHub> hubContext)
    {
        _messageRepository = messageRepository;
        _hubContext = hubContext;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
         var exchangeName = RabbitmqConstants.ResponseExchangeName;
         var queueName = RabbitmqConstants.ResponseQueueName;

         var connectionFactory = new ConnectionFactory()
         {
             HostName = "localhost"
         };
         var connection = await connectionFactory.CreateConnectionAsync(stoppingToken);
         var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
         
         await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, true, false, cancellationToken: stoppingToken);
         await channel.QueueDeclareAsync(queueName, true, false, false, null, cancellationToken: stoppingToken);
         await channel.QueueBindAsync(queueName, exchangeName, RabbitmqConstants.ResponseExchangeRoutingKey, cancellationToken: stoppingToken);

         Console.WriteLine($"Queue: {queueName},  Exchange: {exchangeName}, RoutingKey: {RabbitmqConstants.ResponseExchangeRoutingKey} declared");
         
         var consumer = new AsyncEventingBasicConsumer(channel);
         consumer.ReceivedAsync += async (model, ea) =>
         {
             var msgBytes = ea.Body.ToArray();
             var msgEnvelope = JsonSerializer.Deserialize<MessageEnvelop<string>>(Encoding.UTF8.GetString(msgBytes));
             var rabbitmqResponse = new RabbitmqResponse { CorrelationId = msgEnvelope.CorrelationId ,Payload = msgEnvelope.Payload };
             Console.WriteLine($"Received Message at Queue: @{queueName} :: payload: {rabbitmqResponse.Payload}");
             _messageRepository.Add(rabbitmqResponse);
             Console.WriteLine($"Notification sent to method 'onReceiveResponseSignalr'");
             await _hubContext.Clients.All.SendAsync("onReceiveResponseSignalr", rabbitmqResponse, cancellationToken: stoppingToken);
             await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
         };
         await channel.BasicConsumeAsync(queueName, false, consumer, cancellationToken: stoppingToken);
         Console.WriteLine("Consumer started!");
    }
}



