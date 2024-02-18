using MyWallet.Common.Enums;

namespace Accounts.Domain.ConversionRates.Dtos;

public class ConversionRateDto
{
    public int Id { get; set; }
    public CurrencyEnum SourceCurrency { get; set; }
    public CurrencyEnum DestinationCurrency { get; set; }
    public double Rate { get; set; }
}
