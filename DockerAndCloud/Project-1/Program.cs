


using Project_1.Logger;
var builder = WebApplication.CreateBuilder(args);

var baseDirectory = AppContext.BaseDirectory;

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider());
var prodLogPath = Environment.GetEnvironmentVariable("LOG_PATH");
builder.Logging.AddProvider(new FileLoggerProvider(prodLogPath)); 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();

var app = builder.Build();
app.MapControllers();
app.Run();
