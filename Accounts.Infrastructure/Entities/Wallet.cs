using MyWallet.Common.Enums;
using MyWallet.Common.Models;

namespace Accounts.Infrastructure.Entities;

internal class Wallet : BaseEntity
{
    public int AccountId { get; set; }
    public CurrencyEnum Currency { get; set; }
    public double Balance { get; set; }

    public Account? Account { get; set; }
}
