 using System.Text;
 using System.Text.Json;
 using CoreLibrary.Models;
 using Microsoft.Extensions.Configuration;
 using Project_4.Publisher.Api.Models;
 using RabbitMQ.Client;

namespace Project_4.Publisher.Api.Services;
public interface  IRabbitmqPublishService
{
    Task InitializeAsync();
    void Initialize();
    Task PublishAsync(RequestModel request);
}

public class RabbitmqPublishService : IAsyncDisposable, IRabbitmqPublishService
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IChannel _channel;
    private readonly IConfiguration _configuration;
    public RabbitmqPublishService(IConfiguration configuration)
    {
        var hostName = configuration["HostName"] ?? "localhost";
        _connectionFactory = new ConnectionFactory { HostName = hostName };
        _configuration = configuration;
    }

    public void Initialize()
    {
        InitializeAsync().Wait();
    }
    public async Task InitializeAsync()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await Build();
    }

    private async Task Build()
    {
        var rabbitmqConfig = _configuration.GetSection("Rabbitmq");
        var exchangesConfig = rabbitmqConfig.GetSection("Exchanges");
        foreach (var ex in exchangesConfig.GetChildren())
        {
            var name = ex["Name"];
            var type = ex["Type"];
            var durable = bool.Parse(ex["durable"]);
            await _channel.ExchangeDeclareAsync(name, type, durable, false);
            Console.WriteLine($"Declaring exchange : {name}; type: {type}");
        }
    }
    
    public async Task PublishAsync(RequestModel requestModel)
    {
        Console.WriteLine($"msg: {requestModel.Message} publishing to routingKey: \"{requestModel.RoutingKey}\" to exchange: {requestModel.ExchangeName}...");
        var messageEnvelop = new MessageEnvelop<string>
        {
            CorrelationId = requestModel.CorrelationId,
            Payload = requestModel.Message
        };
        var msgBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageEnvelop));
        await _channel.BasicPublishAsync(requestModel.ExchangeName, requestModel.RoutingKey, false, msgBytes);
    }
    
    public async ValueTask DisposeAsync()
    {
        _channel?.Dispose();
        if (_connection != null)
        {
            await _connection.CloseAsync(); // Close the connection gracefully
            _connection.Dispose();
        }
    }
}