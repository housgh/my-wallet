using MyWallet.Common.Enums;
using MyWallet.Common.Exceptions;
using System.Net;

namespace Accounts.Domain.Wallets.Exceptions;

public class InvalidCurrencyException(CurrencyEnum expectedCurrency, CurrencyEnum actualCurrency) : 
    CustomException($"The provided currency does not match the wallet currency. Expected: {expectedCurrency}, Actual: {actualCurrency}.", HttpStatusCode.Conflict)
{
}
