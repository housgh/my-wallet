using MyWallet.Common.Enums;

namespace Accounts.Infrastructure.Entities;

public class ConversionRate
{
    public int Id { get; set; }
    public CurrencyEnum SourceCurrency { get; set; }
    public CurrencyEnum DestinationCurrency { get; set; }
    public double Rate { get; set; }
}
