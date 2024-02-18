using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWallet.Common.Mappers;
using MyWallet.Common.RabbitMq;

namespace MyWallet.Common.Transactions;

public static class TransactionClientExtensions
{
    public static void AddTransactionRabbitMqClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMqProducer(config =>
        {
            config.HostName = configuration[$"{nameof(TransactionProducer)}:{nameof(RabbitMqConnectionSettings.HostName)}"]!;
            config.Port = configuration.GetValue<int>($"{nameof(TransactionProducer)}:{nameof(RabbitMqConnectionSettings.Port)}")!;
            config.UserName = configuration[$"{nameof(TransactionProducer)}:{nameof(RabbitMqConnectionSettings.UserName)}"]!;
            config.Password = configuration[$"{nameof(TransactionProducer)}:{nameof(RabbitMqConnectionSettings.Password)}"]!;
        });

        services.AddScoped<ITransactionProducer, TransactionProducer>();
    }

    public static void AddTransactionGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<TransactionGrpcService.TransactionGrpcServiceClient>(opt =>
        {
            opt.Address = new Uri(configuration[$"{nameof(TransactionGrpcService)}:{nameof(GrpcClientFactoryOptions.Address)}"]!);
        });

        services.AddScoped<ITransactionGrpcClient, TransactionGrpcClient>();

        services.AddAutoMapper(cfg => cfg.AddProfile<TransactionGrpcMapper>());
    }
}
