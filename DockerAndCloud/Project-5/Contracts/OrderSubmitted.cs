namespace Contracts;

public interface IOrderSubmitted
{
    Guid OrderId { get; set; }
    string CoffeeType { get; set; }
    DateTime Timestamp { get; set; }
}
