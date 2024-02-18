using MyWallet.Common.Interfaces;
using MyWallet.Common.Models;

namespace Transactions.Domain.Transaction.Repositories;

public interface ITransactionRepository : IBaseRepository<TransactionDto>
{
    Task<ICollection<TransactionDto>> GetAllAsync(string customerId, int? accountId = null, int? walletId = null);
}
