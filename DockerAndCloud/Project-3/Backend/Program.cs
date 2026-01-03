
using Project_3.Logger;
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddCors((options) =>
{
    options.AddPolicy("allowAll", (policy) =>
    {
        policy
            .WithOrigins($"{config["Frontend:BaseUrl"]}")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Configuration.AddEnvironmentVariables();
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider());
var prodLogPath = config["Logging:LogPath"];
builder.Logging.AddProvider(new FileLoggerProvider(prodLogPath));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();

var app = builder.Build();
app.UseCors("allowAll");
app.MapControllers();
app.Run();
