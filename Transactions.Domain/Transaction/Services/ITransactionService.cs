using MyWallet.Common.Models;

namespace Transactions.Domain.Transaction.Services;

public interface ITransactionService
{
    Task<(ICollection<TransactionDto> Transactions, double TotalBalance)> GetAllAsync(string customerId, int? accountId = null, int? walledId = null);
    Task AddAsync(TransactionDto transactionDto);
    Task ReverseAsync(int id);
}
