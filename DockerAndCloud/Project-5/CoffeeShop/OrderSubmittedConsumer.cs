using Contracts;
using MassTransit;

namespace CoffeeShop;

public class OrderSubmittedConsumer : IConsumer<IOrderSubmitted>
{
    public async Task Consume(ConsumeContext<IOrderSubmitted> context)
    {
        var message = context.Message;
        Console.WriteLine($"{nameof(OrderSubmittedConsumer)}: Processing order: {message.CoffeeType}");
    }
}