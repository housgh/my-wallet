using Accounts.Domain.Accounts.Dtos;
using Accounts.Domain.Accounts.Repositories;
using Accounts.Domain.Wallets.Dtos;
using Accounts.Infrastructure.DbContexts;
using Accounts.Infrastructure.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyWallet.Common.Repositories;

namespace Accounts.Infrastructure.Repositories;

internal class AccountRepository(AccountDbContext context, IMapper mapper) :
    BaseRepository<AccountDbContext, AccountDto, Account>(context, mapper), IAccountRepository
{
    private readonly AccountDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(AccountDto accountDto, WalletDto initialWallet)
    {
        var account = _mapper.Map<Account>(accountDto);
        var wallet = _mapper.Map<Wallet>(initialWallet);
        account.Wallets = [ wallet ];
        await _context.AddAsync(account);
        await _context.SaveChangesAsync();

        accountDto.Id = account.Id;
        initialWallet.Id = wallet.Id;
    }

    public void DeleteAsync(string customerId, int id)
    {
        _context.Accounts
            .Where(x => x.CustomerId == customerId)
            .Where(x => x.Id == id)
            .ExecuteUpdate(entity =>entity.SetProperty(x => x.IsSoftDeleted, true)
            .SetProperty(x => x.DateModified, DateTime.UtcNow));
    }

    public async Task<bool> ExistsAsync(string customerId, int id)
    {
        return await _context.Accounts
            .Where(x => x.CustomerId == customerId)
           .AnyAsync(x => x.Id == id);
    }

    public async Task<ICollection<AccountDto>> GetAllAsync(string customerId)
    {
        var accounts = await _context.Accounts
            .Where(x => x.CustomerId == customerId)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<ICollection<AccountDto>>(accounts);
    }

    public async Task<AccountDto> GetAsync(string customerId, int id)
    {
        var account = await _context.Accounts
            .Where(x => x.CustomerId == customerId)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return _mapper.Map<AccountDto>(account);
    }

    public async Task RemoveAsync(string customerId, int id)
    {
        await _context.Accounts
            .Where(x => x.CustomerId == customerId)
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(entity => entity.SetProperty(x => x.IsSoftDeleted, true)
            .SetProperty(x => x.DateModified, DateTime.UtcNow));
    }
}
