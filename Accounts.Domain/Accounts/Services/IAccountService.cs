using Accounts.Domain.Accounts.Dtos;

namespace Accounts.Domain.Accounts.Services;

public interface IAccountService
{
    Task<AccountDto> GetAsync(string customerId, int id);
    Task<ICollection<AccountDto>> GetAllAsync(string customerId);
    Task<AccountDto> AddAsync(ExtendedAccountDto extendedWalletDto);
    Task<AccountDto> UpdateStatusAsync(string customerId, int accountId);
    Task RemoveAsync(string customerId, int id);
    Task<MiniStatementDto> GetMiniStatementAsync(string customerId, int? accountId = null, int? walletId = null);
}
