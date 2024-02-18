using AutoMapper;
using MyWallet.Common;
using MyWallet.Common.Models;

namespace MyWallet.Common.Mappers;

public class TransactionGrpcMapper : Profile
{
    public TransactionGrpcMapper()
    {
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.FromBinary(src.DateCreated)))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => DateTime.FromBinary(src.DateModified)))
            .ReverseMap()
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.ToBinary()))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateModified.ToBinary()));
    }
}
