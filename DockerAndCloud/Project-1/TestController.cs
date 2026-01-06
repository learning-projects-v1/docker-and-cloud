using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;

namespace Project_1;

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
        // var configPath = _configuration.GetSection("Config:ConfigPath").Value;
        var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        Console.WriteLine($"ConfigPath: {path ?? "not found"}");
        var values = System.IO.File.ReadAllText(path);
        Console.WriteLine(values);
        return values;
         
        return "Path doesn't exist";
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
    public string Test()
    {
        return "Running";
    }

    [HttpGet("environment")]
    public object GetEnvironment()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var prodLogPath = Environment.GetEnvironmentVariable("LogPath");
        return new
        {
            environment = environment,
            prodLogPath = prodLogPath
        };
    }

    [HttpPost("log")]
    public void PostLog([FromBody]string msg)
    {
        _logger.LogInformation(msg);
    }

    [HttpGet("log")]
    public string GetLogs()
    {
        var logPath = Environment.GetEnvironmentVariable("LOG_PATH");
        var values = System.IO.File.ReadAllText(logPath);
        return values;
    }
}