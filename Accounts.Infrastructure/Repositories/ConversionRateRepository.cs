using Accounts.Domain.ConversionRates.Dtos;
using Accounts.Domain.ConversionRates.Repositories;
using Accounts.Infrastructure.DbContexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyWallet.Common.Enums;

namespace Accounts.Infrastructure.Repositories;

internal class ConversionRateRepository : IConversionRateRepository
{
    private readonly AccountDbContext _context;
    private readonly IMapper _mapper;

    public ConversionRateRepository(
        AccountDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ConversionRateDto> GetAsync(CurrencyEnum sourceCurrency, CurrencyEnum destinationCurrency)
    {
        var conversionRate = await _context.ConversionRates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SourceCurrency == sourceCurrency && 
            x.DestinationCurrency == destinationCurrency);
        return _mapper.Map<ConversionRateDto>(conversionRate);
    }
}
