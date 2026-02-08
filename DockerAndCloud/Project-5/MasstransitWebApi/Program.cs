using Contracts;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddMassTransit(x =>
{
    x.AddRequestClient<CheckInventory>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});
var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();