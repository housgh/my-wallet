using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Transactions.Domain.Transaction.Repositories;
using Transactions.Infrastructure.DbContexts;
using Transactions.Infrastructure.Mappers;
using Transactions.Infrastructure.Repositories;

namespace Transactions.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddTransactionInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransactionDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString(nameof(TransactionDbContext)));
        });

        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddAutoMapper(cfg => cfg.AddProfile<EntityToDtoMapper>());

        var context = services.BuildServiceProvider().GetRequiredService<TransactionDbContext>();

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development" 
            && context.Database.GetPendingMigrations().Any())
        {
            Console.WriteLine("Migrating Database...");
            context.Database.Migrate();
        }
    }
}
