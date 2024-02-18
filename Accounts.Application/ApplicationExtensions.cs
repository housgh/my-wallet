using Accounts.Application.Services;
using Accounts.Domain.Accounts.Services;
using Accounts.Domain.Wallets.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWallet.Common.Customers;
using MyWallet.Common.Transactions;

namespace Accounts.Application;

public static class ApplicationExtensions
{
    public static void AddAccountsApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IAccountService, AccountService>();

        services.AddTransactionRabbitMqClient(configuration);
        services.AddTransactionGrpcClient(configuration);

        services.AddCustomerGrpcClient(configuration);
    }
}
