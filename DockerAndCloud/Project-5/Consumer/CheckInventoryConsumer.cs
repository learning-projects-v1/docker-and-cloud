using Contracts;
using MassTransit;

public class CheckInventoryConsumer : IConsumer<CheckInventory>
{
    private static int _attemptCount;

    public async Task Consume(ConsumeContext<CheckInventory> context)
    {
        _attemptCount++;
        if (_attemptCount <= 2)
        {
            Console.WriteLine($"[Consumer] Attempt {_attemptCount}: Database is busy! Throwing error...");
            throw new Exception("Database connection failed!");
        }
        
        var available = context.Message.CoffeeType == "DarkRoast";
        if (!available)
        {
            await context.RespondAsync(new OrderNotFound(){CoffeeType = context.Message.CoffeeType});
        }
        Console.WriteLine($"{nameof(CheckInventoryConsumer)}: Processing Request: {context.Message.CoffeeType}");

        await context.RespondAsync<InventoryStatus>(new()
        {
            InStock = available,
            Price = 23.332
        });
    }
}