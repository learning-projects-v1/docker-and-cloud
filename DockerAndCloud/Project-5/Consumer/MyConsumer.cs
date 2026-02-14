using Contracts;
using MassTransit;

namespace Consumer;

public class MyConsumer : IConsumer<IOrderSubmitted>
{
    private static int _counter { get; set; }
    public async Task Consume(ConsumeContext<IOrderSubmitted> context)
    {
        Console.WriteLine("At MyConsumer");
        _counter++;
        if (_counter <= 2)
        {
            Console.WriteLine("Error! Counter: {0}", _counter);
            throw new Exception("Something went wrong");
        }
        var message = context.Message;
        Console.WriteLine("{0} Passed!", _counter);
        Console.WriteLine($"{nameof(MyConsumer)}: Processing order: {message.CoffeeType}");
    }
}