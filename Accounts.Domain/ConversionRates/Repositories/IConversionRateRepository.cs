using Accounts.Domain.ConversionRates.Dtos;
using MyWallet.Common.Enums;

namespace Accounts.Domain.ConversionRates.Repositories;

public interface IConversionRateRepository
{
    Task<ConversionRateDto> GetAsync(CurrencyEnum sourceCurrency, CurrencyEnum destinationCurrency);
}
