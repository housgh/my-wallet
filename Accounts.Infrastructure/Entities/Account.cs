using Accounts.Domain.Accounts.Enums;
using MyWallet.Common.Models;

namespace Accounts.Infrastructure.Entities;

internal class Account : BaseEntity
{
    public string CustomerId { get; set; } = string.Empty;
    public AccountTypeEnum AccountType { get; set; }

    public ICollection<Wallet>? Wallets { get; set; }
}
