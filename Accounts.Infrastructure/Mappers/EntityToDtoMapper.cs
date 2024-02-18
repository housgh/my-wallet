using Accounts.Domain.Accounts.Dtos;
using Accounts.Domain.Wallets.Dtos;
using Accounts.Infrastructure.Entities;
using AutoMapper;

namespace Accounts.Infrastructure.Mappers;

internal class EntityToDtoMapper : Profile
{
    public EntityToDtoMapper() 
    { 
        CreateMap<Wallet, WalletDto>().ReverseMap();
        CreateMap<Wallet, ExtendedWalletDto>().
            ForMember(src => src.AccountType, opt => opt.MapFrom(dest => dest.Account!.AccountType))
            .ForMember(src => src.CustomerId, opt => opt.MapFrom(dest => dest.Account!.CustomerId));
        CreateMap<Account, AccountDto>().ReverseMap();
    }
}
