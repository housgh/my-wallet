using MyWallet.Common.Exceptions;

namespace Transactions.Domain.Transaction.Exceptions;

public class TransactionNotFoundException() : NotFoundException($"The requested transaction does not exist.")
{
}
