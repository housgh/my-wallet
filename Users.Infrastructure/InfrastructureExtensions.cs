using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Domain.Users.Repositories;
using Users.Infrastructure.DbContexts;
using Users.Infrastructure.Mappers;
using Users.Infrastructure.Repositories;

namespace Users.Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddUsersInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString(nameof(UserDbContext)));
        });

        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddAutoMapper(cfg => cfg.AddProfile<EntityToDtoMapper>());

        var context = services.BuildServiceProvider().GetRequiredService<UserDbContext>();

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development"
            && context.Database.GetPendingMigrations().Any())
        {
            Console.WriteLine("Migrating Database...");
            context.Database.Migrate();
        }
    }
}
