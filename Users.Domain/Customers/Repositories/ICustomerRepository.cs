using MyWallet.Common.Interfaces;
using MyWallet.Common.Models;

namespace Users.Domain.Users.Repositories;

public interface ICustomerRepository : IBaseRepository<CustomerDto>
{
    Task<bool> ExistsAsync(string customerId);
    Task<CustomerDto> GetAsync(string customerId);
}
