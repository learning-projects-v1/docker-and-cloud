namespace Project_4.Publisher.Api.Models;

public class RequestModel
{
    public string ExchangeType { get; set; }
    public string ExchangeName { get; set; }
    public string RoutingKey { get; set; }
    public string Message { get; set; }
    public string CorrelationId { get; set; }
}