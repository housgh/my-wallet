using AutoMapper;
using MyWallet.Common.Models;
using Users.Infrastructure.Entities;

namespace Users.Infrastructure.Mappers;

internal class EntityToDtoMapper : Profile
{
    public EntityToDtoMapper()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
    }
}
