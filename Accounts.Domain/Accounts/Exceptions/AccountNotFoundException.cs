using MyWallet.Common.Exceptions;

namespace Accounts.Domain.Accounts.Exceptions;

public class AccountNotFoundException(int accountId) : 
    NotFoundException($"Account with ID {accountId} not found.")
{
}
