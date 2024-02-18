using MyWallet.Common.Customers;
using MyWallet.Common.Models;

namespace Accounts.Application.Services;

public interface IUserService
{
    Task AddAsync(CustomerDto customer);
}

internal class UserService : IUserService
{
    private readonly ICustomerGrpcClient _customerGrpcClient;

    public UserService(ICustomerGrpcClient customerGrpcClient)
    {
        _customerGrpcClient = customerGrpcClient;
    }

    public async Task AddAsync(CustomerDto customer)
    {
        await _customerGrpcClient.AddAsync(customer);
    }
}
