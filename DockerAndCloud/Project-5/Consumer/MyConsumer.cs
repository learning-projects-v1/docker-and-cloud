using Contracts;
using MassTransit;

namespace Consumer;

public class MyConsumer : IConsumer<IOrderSubmitted>
{
    public async Task Consume(ConsumeContext<IOrderSubmitted> context)
    {
        var message = context.Message;
        Console.WriteLine($"{nameof(MyConsumer)}: Processing order: {message.CoffeeType}");
    }
}