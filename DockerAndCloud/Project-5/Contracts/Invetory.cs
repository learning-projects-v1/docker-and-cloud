namespace Contracts;


public record CheckInventory
{
    public string CoffeeType { get; init; }
}

public record InventoryStatus
{
    public bool InStock { get; init; }
    public double Price { get; init; }
}