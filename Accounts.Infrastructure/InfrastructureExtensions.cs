using Accounts.Domain.Accounts.Repositories;
using Accounts.Domain.ConversionRates.Repositories;
using Accounts.Domain.Wallets.Repositories;
using Accounts.Infrastructure.DbContexts;
using Accounts.Infrastructure.Mappers;
using Accounts.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Accounts.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddAccountsInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AccountDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString(nameof(AccountDbContext)));
        });

        services.AddAutoMapper(config => config.AddProfile<EntityToDtoMapper>());

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IConversionRateRepository, ConversionRateRepository>();

        var context = services.BuildServiceProvider().GetRequiredService<AccountDbContext>();
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development"
            && context.Database.GetPendingMigrations().Any())
        {
            Console.WriteLine("Migrating Database...");
            context.Database.Migrate();
        }
    }
}
