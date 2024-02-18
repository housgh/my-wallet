
using MyWallet.Common.Enums;
using MyWallet.Common.Models;

namespace Accounts.Domain.Wallets.Dtos;

public class WalletDto : BaseEntityDto
{
    public int AccountId { get; set; }
    public CurrencyEnum Currency { get; set; }
    public double Balance { get; set; }
    public bool Active { get; set; }
    public double CreditLimit { get; set; }
}
