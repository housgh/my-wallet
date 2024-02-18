using MyWallet.Common.Exceptions;

namespace Accounts.Domain.Wallets.Exceptions;

public class WalletNotFoundException(int walletId) : 
    NotFoundException($"Wallet with id {walletId} not found.")
{
}
