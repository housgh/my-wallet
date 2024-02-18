using Accounts.Domain.Wallets.Dtos;
using Accounts.Domain.Wallets.Repositories;
using Accounts.Infrastructure.DbContexts;
using Accounts.Infrastructure.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyWallet.Common.Enums;
using MyWallet.Common.Repositories;

namespace Accounts.Infrastructure.Repositories
{
    internal class WalletRepository(AccountDbContext context, IMapper mapper) : 
        BaseRepository<AccountDbContext, WalletDto, Wallet>(context, mapper), IWalletRepository
    {
        private readonly AccountDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public void Delete(string customerId, int accountId, int id)
        {
            _context.Wallets
                .Where(x => x.Account!.CustomerId == customerId)
                .Where(x => x.AccountId == accountId)
                .Where(x => x.Id == id)
                .ExecuteUpdate(entity => entity.SetProperty(x => x.IsSoftDeleted, true)
                .SetProperty(x => x.DateModified, DateTime.UtcNow));
        }

        public async Task<bool> ExistsAsync(string customerId, int accountId, int id)
        {
            return await _context.Wallets
                .Where(x => x.Account!.CustomerId == customerId)
                .Where(x => x.AccountId == accountId)
                .AnyAsync(x => x.Id == id);
        }

        public async Task<ICollection<WalletDto>> GetAllAsync(string customerId, int? accountId = null, CurrencyEnum? currency = null)
        {
            var wallets = await _context.Wallets
                .Where(x => x.Account!.CustomerId == customerId)
                .Where(x => accountId == null || x.AccountId == accountId)
                .Where(x => currency == null || x.Currency == currency)
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<ICollection<WalletDto>>(wallets);
        }

        public async Task<WalletDto> GetAsync(string customerId, int accountId, int id)
        {
            var wallet = await _context.Wallets
                .AsNoTracking()
                .Where(x => x.Account!.CustomerId == customerId)
                .Where(x => x.AccountId == accountId)
                .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<WalletDto>(wallet);
        }

        public async Task<ExtendedWalletDto> GetWalletWithAccount(string customerId, int accountId, int walletId)
        {
            var wallet = await _context.Wallets
                .Include(x => x.Account)
                .AsNoTracking()
                .Where(x => x.Account!.CustomerId == customerId)
                .Where(x => x.AccountId == accountId)
                .FirstOrDefaultAsync(x => x.Id == walletId);
            return _mapper.Map<ExtendedWalletDto>(wallet);
        }

        public async Task<bool> WalletExistsAsync(int accountId, CurrencyEnum currency)
        {
            return await _context.Wallets
                .Where(x => x.AccountId == accountId)
                .AnyAsync(x => x.Currency == currency);
        }
    }
}
