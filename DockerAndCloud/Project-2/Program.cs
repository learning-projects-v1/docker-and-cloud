


using Project_1.Logger;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors((options) =>
{
    options.AddPolicy("allowAll", (policy) =>
    {
        policy
            .WithOrigins($"{Environment.GetEnvironmentVariable("FRONTEND_BASE")}")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider());
var prodLogPath = Environment.GetEnvironmentVariable("LOG_PATH");
builder.Logging.AddProvider(new FileLoggerProvider(prodLogPath)); 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();

var app = builder.Build();
app.UseCors("allowAll");
app.MapControllers();
app.Run();
