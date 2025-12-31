namespace Project_1.Logger;

public class CustomLoggerProvider : ILoggerProvider
{
    public void Dispose()
    {
        // TODO release managed resources here
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new CustomLogger(categoryName);
    }
}

public class FileLoggerProvider(string filePath) : ILoggerProvider
{
    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryNam)
    {
        // if (!File.Exists(filePath))
        // {
        //     File.Create(filePath);
        // }
        // File.Open(filePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
        return new FileLogger(filePath);
    }
}