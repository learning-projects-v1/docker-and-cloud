namespace Project_1.Logger;

public class FileLogger(string filePath) : ILogger
{
    private static readonly object _lock = new object();
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None && logLevel != LogLevel.Trace;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;
        var logMessage = formatter(state, exception);
        var logRecord = $"{logLevel}: {logMessage}";
        lock (_lock)
        {
            File.AppendAllText(filePath, logRecord);
            File.AppendAllText(filePath, Environment.NewLine);
        }
    }
}