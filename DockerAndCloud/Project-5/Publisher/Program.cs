// See https://aka.ms/new-console-template for more information

using CoffeeShop;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var services = builder.Services;

services.AddMassTransit(x =>
{
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
        cfg.ConfigureEndpoints(context);
    });
});

services.AddHostedService<MyPublisher>();
var app = builder.Build();
await app.RunAsync();