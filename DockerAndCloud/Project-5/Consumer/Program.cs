// See https://aka.ms/new-console-template for more information

using Consumer;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var services = builder.Services;

services.AddMassTransit(x =>
{
    x.AddConsumer<MyConsumer>();
    x.AddConsumer<CheckInventoryConsumer>();
    // x.UsingInMemory((context, cfg) =>
    // {
    //     cfg.Host("localhost");
    //     cfg.ConfigureEndpoints(context);
    // });
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.UseMessageRetry(abc =>
        {
            abc.Interval(3, TimeSpan.FromSeconds(4));
        });
        cfg.ConfigureEndpoints(context);
        
    });
});

// services.AddHostedService<OrderSubmittedMesssagePublisher>();

var app = builder.Build();
await app.RunAsync();