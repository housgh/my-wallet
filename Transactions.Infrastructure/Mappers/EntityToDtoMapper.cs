using AutoMapper;
using MyWallet.Common.Models;
using Transactions.Infrastructure.Entities;

namespace Transactions.Infrastructure.Mappers;

internal class EntityToDtoMapper : Profile
{
    public EntityToDtoMapper() 
    {
        CreateMap<Transaction, TransactionDto>().ReverseMap();
    }
}
