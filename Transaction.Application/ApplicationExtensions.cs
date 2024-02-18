using Microsoft.Extensions.DependencyInjection;
using Transactions.Application.Services;
using Transactions.Domain.Transaction.Services;

namespace Transactions.Application;

public static class ApplicationExtensions
{
    public static void AddTransactionsApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITransactionService, TransactionService>();
    }
}
