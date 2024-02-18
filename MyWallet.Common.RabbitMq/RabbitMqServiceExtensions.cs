using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MyWallet.Common.RabbitMq;

public static class RabbitMqServiceExtensions
{
    public static void AddRabbitMqProducer(this IServiceCollection services, Action<RabbitMqConnectionSettings> action)
    {
        RegisterConnectionFactory(services, action);
        services.AddScoped<IRabbitMqProducer, RabbitMqProducer>();
    }

    public static void AddRabbitMqConsumer(this IServiceCollection services, Action<RabbitMqConnectionSettings> action)
    {
        RegisterConnectionFactory(services, action);
        services.AddScoped<IRabbitMqConsumer, RabbitMqConsumer>();
    }

    private static void RegisterConnectionFactory(IServiceCollection services, Action<RabbitMqConnectionSettings> action)
    {
        var settings = new RabbitMqConnectionSettings(action);
        Console.WriteLine($"Connecting to RabbitMq: {settings.HostName}:{settings.Port}...");
        services.AddSingleton<IConnectionFactory>(new ConnectionFactory
        {
            HostName = settings.HostName,
            Port = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password
        });
    }
}
