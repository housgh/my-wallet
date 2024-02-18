using MyWallet.Common.Enums;
using MyWallet.Common.Exceptions;

namespace Accounts.Domain.Wallets.Exceptions;

public class WalletAlreadyExistsException(CurrencyEnum currency) : 
    CustomException($"A {currency} wallet already exists for this account.")
{
}
