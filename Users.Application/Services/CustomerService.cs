using MyWallet.Common.Exceptions;
using MyWallet.Common.Models;
using Users.Domain.Customers.Services;
using Users.Domain.Users.Repositories;

namespace Users.Application.Services;

internal class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task AddAsync(CustomerDto customerDto)
    {
        await _repository.AddAsync(customerDto);
    }

    public async Task<bool> ExistsAsync(string customerId)
    {
        return await _repository.ExistsAsync(customerId);
    }

    public Task<CustomerDto> GetAsync(string customerId)
    {
        var customer = _repository.GetAsync(customerId) ?? 
            throw new CustomerNotFoundException(customerId);
        return customer;
    }
}
