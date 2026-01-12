using System.Collections.Concurrent;

namespace Project_4.Publisher.Api.Repository;

public interface IMessageRepository<T>
{
    void Add(T message);
    List<T> Drain();
}
public class MessageRepository<T> : IMessageRepository<T>
{
    private readonly ConcurrentQueue<T> _messages = new();

    public void Add(T message)
    {
        _messages.Enqueue(message);
    }

    public List<T> Drain()
    {
        var result = new List<T>();
        while (_messages.TryDequeue(out var message))
        {
            if (message != null)
            {
                result.Add(message);
            }
        }
        return result;
    }
}