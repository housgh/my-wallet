using Accounts.Domain.Accounts.Dtos;
using Accounts.Domain.Wallets.Dtos;
using MyWallet.Common.Interfaces;

namespace Accounts.Domain.Accounts.Repositories;

public interface IAccountRepository : IBaseRepository<AccountDto>
{
    Task<AccountDto> GetAsync(string customerId, int id);
    Task<ICollection<AccountDto>> GetAllAsync(string customerId);
    Task AddAsync(AccountDto accountDto, WalletDto initialWallet);
    Task RemoveAsync(string customerId, int id);
    Task<bool> ExistsAsync(string customerId, int id);
    void DeleteAsync(string customerId, int id);
}
