using MyWallet.Common.Enums;
using MyWallet.Common.Exceptions;
using MyWallet.Common.Models;

namespace Accounts.Domain.Wallets.Exceptions;

public class InsufficientFundException(Money requestedAmount) : 
    CustomException($"Your transaction for {requestedAmount.Amount}{requestedAmount.Currency.ToString().ToUpper()} has been rejected due to insufficient funds.")
{
}
