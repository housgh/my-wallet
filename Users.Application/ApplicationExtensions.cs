using Microsoft.Extensions.DependencyInjection;
using Users.Application.Services;
using Users.Domain.Customers.Services;

namespace Users.Application;

public static class ApplicationExtensions
{
    public static void AddUserApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
    }
}
