using MyWallet.Common.Exceptions;
using MyWallet.Common.Models;

namespace Accounts.Domain.Wallets.Exceptions;

public class CreditLimitExceededException(Money requestedAmount) : 
    CustomException($"Your transaction for {requestedAmount.Amount}{requestedAmount.Currency.ToString().ToUpper()} has been rejected because you have exceeded your credit limit.")
{
}
