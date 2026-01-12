using System.ComponentModel.DataAnnotations;

namespace Project_4.Publisher.Api.Models;

public class RequestModel
{
    public string? ExchangeType { get; set; }
    public string? ExchangeName { get; set; }
    public string? RoutingKey { get; set; }
    public required string Message { get; set; }
    public required string CorrelationId { get; set; }
}