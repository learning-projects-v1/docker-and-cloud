namespace CoreLibrary.Models;

public class MessageEnvelop<T>
{
    public T Payload { get; set; }
    public string CorrelationId { get; set; }
}