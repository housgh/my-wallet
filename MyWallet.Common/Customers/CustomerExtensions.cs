using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWallet.Common.Mappers;

namespace MyWallet.Common.Customers;

public static class CustomerExtensions
{
    public static void AddCustomerGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICustomerGrpcClient, CustomerGrpcClient>();

        services.AddAutoMapper(cfg => cfg.AddProfile<CustomerGrpcMapper>());

        services.AddGrpcClient<CustomerGrpcService.CustomerGrpcServiceClient>(opt =>
        {
            opt.Address = new Uri(configuration[$"{nameof(CustomerGrpcService)}:{nameof(GrpcClientFactoryOptions.Address)}"]!);
        });
    }
}
