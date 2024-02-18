using MyWallet.Common.Models;
using Transactions.Domain.Transaction.Exceptions;
using Transactions.Domain.Transaction.Repositories;
using Transactions.Domain.Transaction.Services;

namespace Transactions.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;

    public TransactionService(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<(ICollection<TransactionDto> Transactions, double TotalBalance)> GetAllAsync(string customerId, int? accountId = null, int? walledId = null)
    {
        var transactions = await _repository.GetAllAsync(customerId, accountId, walledId);
        double total = 0;
        total += transactions.Where(x => x.DestinationCustomerId == customerId).Sum(x => x.Amount);
        total -= transactions.Where(x => x.SourceCustomerId == customerId).Sum(x => x.Amount);
        return (transactions, total);
    }

    public async Task AddAsync(TransactionDto transactionDto)
    {
        await _repository.AddAsync(transactionDto);
    }

    public async Task ReverseAsync(int id)
    {
        var transaction = await _repository.GetAsync(id) ?? 
            throw new TransactionNotFoundException();
        var reversedTransaction = new TransactionDto
        {
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
            DestinationAccountId = transaction.SourceAccountId,
            DestinationCustomerId = transaction.SourceCustomerId,
            DestinationWalletId = transaction.SourceWalletId,
            SourceAccountId = transaction.DestinationAccountId,
            SourceCustomerId = transaction.DestinationCustomerId,
            SourceWalletId = transaction.DestinationWalletId,
            Type = transaction.Type,
            TraceId = transaction.TraceId,
            IsReverse = true
        };
        await _repository.AddAsync(reversedTransaction);
    }
}
