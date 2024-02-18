using MyWallet.Common.Enums;

namespace Accounts.Domain.Accounts.Dtos;

public class ExtendedAccountDto : AccountDto
{
    public CurrencyEnum Currency { get; set; }
    public double Balance { get; set; }
    public double CreditLimit { get; set; }
}
