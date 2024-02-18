using Accounts.Domain.Accounts.Enums;
using Accounts.Domain.Wallets.Dtos;
using MyWallet.Common.Models;

namespace Accounts.Domain.Accounts.Dtos;

public class AccountDto : BaseEntityDto
{
    public string CustomerId { get; set; } = string.Empty;
    public AccountTypeEnum AccountType { get; set; }
    public bool Active { get; set; }
}
