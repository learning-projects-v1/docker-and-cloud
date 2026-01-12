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
    private readonly ConnectionFactory _factory;
    private IConnection _connection;
    private IChannel _channel;
    private readonly IConfiguration _configuration;
    public RabbitmqPublishService(IConfiguration configuration)
    {
        var hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST")
                       ?? throw new InvalidOperationException("RABBITMQ_HOST environment variable is not set");

        _factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest",
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest",
        };
        _configuration = configuration;
    }

    public void Initialize()
    {
        InitializeAsync().Wait();
    }
    public async Task InitializeAsync()
    {
        IConnection connection = null;
        Console.WriteLine("Connecting to RabbitMQ...");
        while (connection == null)
        {
            try
            {
                Console.WriteLine("Connecting to RabbitMQ...");
                connection = await _factory.CreateConnectionAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ not ready: {ex.Message}");
                await Task.Delay(5000);
            }
        }
        _channel = await connection.CreateChannelAsync();

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