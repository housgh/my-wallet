using AutoMapper;
using MyWallet.Common.Models;

namespace MyWallet.Common.Customers;

public interface ICustomerGrpcClient
{
    Task AddAsync(CustomerDto customerDto);
    Task<bool> ExistsAsync(string customerId);
    Task<CustomerDto> GetAsync(string customerId);
}

internal class CustomerGrpcClient : ICustomerGrpcClient
{
    private readonly CustomerGrpcService.CustomerGrpcServiceClient _customerGrpcClient;
    private readonly IMapper _mapper;

    public CustomerGrpcClient(
        CustomerGrpcService.CustomerGrpcServiceClient customerGrpcClient,
        IMapper mapper)
    {
        _customerGrpcClient = customerGrpcClient;
        _mapper = mapper;
    }

    public async Task AddAsync(CustomerDto customerDto)
    {
        var customer = _mapper.Map<Customer>(customerDto);
        await _customerGrpcClient.AddAsync(customer);
    }

    public async Task<bool> ExistsAsync(string customerId)
    {
        var response = await _customerGrpcClient.ExistsAsync(new CustomerRequest { CustomerId = customerId });
        return response.Exists;
    }

    public async Task<CustomerDto> GetAsync(string customerId)
    {
        var response = await _customerGrpcClient.GetAsync(new CustomerRequest { CustomerId = customerId });
        return _mapper.Map<CustomerDto>(response);
    }
}
