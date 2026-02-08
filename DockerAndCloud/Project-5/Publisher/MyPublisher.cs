using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace CoffeeShop;

public class MyPublisher : BackgroundService
{
    private readonly IBus _bus;
    public MyPublisher(IBus bus)
    {
        _bus = bus;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Publishing Message from: " + nameof(MyPublisher));
            await _bus.Publish<IOrderSubmitted>(new
            {
                OrderId = Guid.NewGuid(),
                CoffeeType = "Capuchino",
                Timestamp = DateTime.UtcNow
            });
            await Task.Delay(1000, stoppingToken);
        }
    }
}