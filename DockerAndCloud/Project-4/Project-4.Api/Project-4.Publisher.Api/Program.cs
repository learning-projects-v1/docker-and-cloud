
using CoreLibrary.Models;
using Project_4.Publisher.Api.Repository;
using Project_4.Publisher.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCors((options) =>
{
    options.AddPolicy("allowAll", (policy) =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSingleton<IRabbitmqPublishService, RabbitmqPublishService>();
builder.Services.AddHostedService<RabbitmqInitializerService>();
builder.Services.AddHostedService<RabbitmqConsumerService>();
builder.Services.AddSingleton(
    typeof(IMessageRepository<>),
    typeof(MessageRepository<>)
);
builder.Services.AddSignalR();
var app = builder.Build();

// That's one way to intialze async startups
// var rabbitmqService = app.Services.GetRequiredService<IRabbitmqPublishService>();
// await rabbitmqService.InitializeAsync();

app.UseRouting()
    .UseCors("allowAll");
app.MapControllers();
app.MapHub<NotificaitonHub>("signalr");
app.Run();