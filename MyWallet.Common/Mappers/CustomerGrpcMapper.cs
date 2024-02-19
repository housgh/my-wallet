using AutoMapper;
using MyWallet.Common.Models;

namespace MyWallet.Common.Mappers;

public class CustomerGrpcMapper : Profile
{
    public CustomerGrpcMapper()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
    }
}
