using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace CoffeeShop;

public class OrderSubmittedMesssagePublisher : BackgroundService
{
    private readonly IBus _bus;
    public OrderSubmittedMesssagePublisher(IBus bus)
    {
        _bus = bus;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
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