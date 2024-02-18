using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyWallet.Common.Models;
using MyWallet.Common.Repositories;
using Transactions.Domain.Transaction.Repositories;
using Transactions.Infrastructure.DbContexts;
using Transactions.Infrastructure.Entities;

namespace Transactions.Infrastructure.Repositories;

internal class TransactionRepository : BaseRepository<TransactionDbContext, TransactionDto, Transaction>, ITransactionRepository
{
    private readonly TransactionDbContext _context;
    private readonly IMapper _mapper;

    public TransactionRepository(TransactionDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ICollection<TransactionDto>> GetAllAsync(string customerId, int? accountId = null, int? walletId = null)
    {
        var transactions = await _context.Transactions
            .Where(x => x.SourceCustomerId == customerId || x.DestinationCustomerId == customerId)
            .Where(x => accountId == null || x.SourceAccountId == accountId || x.DestinationAccountId == accountId)
            .Where(x => walletId == null || x.SourceWalletId == walletId || x.DestinationWalletId == walletId)
            .AsNoTracking()
            .ToListAsync();
        return _mapper.Map<ICollection<TransactionDto>>(transactions);
    }
}
