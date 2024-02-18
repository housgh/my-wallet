using Accounts.Domain.Accounts.Enums;

namespace Accounts.Domain.Wallets.Dtos;

public class ExtendedWalletDto : WalletDto
{
    public AccountTypeEnum AccountType { get; set; }
    public required string CustomerId { get; set; }
}
