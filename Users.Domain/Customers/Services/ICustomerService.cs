using MyWallet.Common.Models;

namespace Users.Domain.Customers.Services;

public interface ICustomerService
{
    Task<bool> ExistsAsync(string customerId);
    Task<CustomerDto> GetAsync(string customerId);
    Task AddAsync(CustomerDto customerDto);
}
