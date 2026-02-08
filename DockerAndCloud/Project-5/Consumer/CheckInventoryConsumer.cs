using Contracts;
using MassTransit;

public class CheckInventoryConsumer : IConsumer<CheckInventory>
{
    public async Task Consume(ConsumeContext<CheckInventory> context)
    {
        var available = context.Message.CoffeeType == "DarkRoast";
        Console.WriteLine($"{nameof(CheckInventoryConsumer)}: Processing Request: {context.Message.CoffeeType}");

        await context.RespondAsync<InventoryStatus>(new()
        {
            InStock = available,
            Price = 23.332
        });
    }
}