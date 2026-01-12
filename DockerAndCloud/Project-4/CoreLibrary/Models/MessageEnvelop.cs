using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Models;

public class MessageEnvelop<T>
{
    public required T Payload { get; set; }
    public required string CorrelationId { get; set; }
}