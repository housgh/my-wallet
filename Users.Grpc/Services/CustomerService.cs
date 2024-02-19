using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MyWallet.Common;
using MyWallet.Common.Models;
using Users.Domain.Customers.Services;

namespace Users.Grpc.Services;

public class CustomerService : CustomerGrpcService.CustomerGrpcServiceBase
{
    private readonly ICustomerService _customerService;
    private readonly IMapper _mapper;

    public CustomerService(
        ICustomerService customerService,
        IMapper mapper)
    {
        _customerService = customerService;
        _mapper = mapper;
    }

    public override async Task<Empty> Add(Customer request, ServerCallContext context)
    {
        var customer = _mapper.Map<CustomerDto>(request);
        await _customerService.AddAsync(customer);
        return new Empty();
    }

    public override async Task<CustomerExistsResponse> Exists(CustomerRequest request, ServerCallContext context)
    {
        var exists = await _customerService.ExistsAsync(request.CustomerId);
        return new CustomerExistsResponse { Exists = exists };
    }

    public override async Task<Customer> Get(CustomerRequest request, ServerCallContext context)
    {
        var customer = await _customerService.GetAsync(request.CustomerId);
        return _mapper.Map<Customer>(customer) ?? new Customer();
    }
}
