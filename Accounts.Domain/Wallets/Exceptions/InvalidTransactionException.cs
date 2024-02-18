using MyWallet.Common.Exceptions;

namespace Accounts.Domain.Wallets.Exceptions;

public class InvalidTransactionException() : 
    CustomException($"Invalid transaction; You can only credit into a debit account.")
{
}
