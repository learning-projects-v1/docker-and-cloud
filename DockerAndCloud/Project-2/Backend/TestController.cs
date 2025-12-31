using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;

namespace Project_2;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TestController> _logger;
    public TestController(IConfiguration configuration, ILogger<TestController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    [HttpGet("file-read")]
    public string GetFileContents()
    {
        const string fileName = "dummy.json";
        var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        Console.WriteLine($"ConfigPath: {path ?? "not found"}");
        var values = System.IO.File.ReadAllText(path);
        Console.WriteLine(values);
        return values;
    }

    [HttpGet("system")]
    public object GetSystemInfo()
    {
        string osDesc = RuntimeInformation.OSDescription;
        Architecture arch = RuntimeInformation.ProcessArchitecture;
        return new
        {
            OsDescription = osDesc,
            Architecture = arch.ToString()
        };
    }

    [HttpGet("")]
    public object Test()
    {
        return new{ Status = "Running"};
    }

    [HttpGet("environment")]
    public object GetEnvironment()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var logPath = _configuration["Logging:LogPath"];
        var url = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
        return new
        {
            environment = environment,
            prodLogPath = logPath,
            rootUrl = url
        };
    }

    [HttpPost("log")]
    public void PostLog([FromBody]string msg)
    {
        _logger.LogCritical(msg);
    }

    [HttpGet("log")]
    public object GetLogs()
    {
        var logPath = _configuration["Logging:LogPath"];
        var values = System.IO.File.ReadAllText(logPath);
        var split = values.Split("\n");
        
        return new{ values = split };
    }

    [HttpDelete("log")]
    public void DeleteLog()
    {
        var logPath = _configuration["Logging:LogPath"];
        var values = System.IO.File.ReadAllText(logPath);
        System.IO.File.WriteAllText(logPath, "");
    }
}