using Accounts.Domain.Wallets.Dtos;
using MyWallet.Common.Enums;
using MyWallet.Common.Interfaces;

namespace Accounts.Domain.Wallets.Repositories;

public interface IWalletRepository : IBaseRepository<WalletDto>
{
    Task<WalletDto> GetAsync(string customerId, int accountId, int id);
    Task<ExtendedWalletDto> GetWalletWithAccount(string customerId, int accountId, int walletId);
    Task<ICollection<WalletDto>> GetAllAsync(string customerId, int? accountId = null, CurrencyEnum? currency = null);
    Task<bool> WalletExistsAsync(int accountId, CurrencyEnum currency);
    Task<bool> ExistsAsync(string customerId, int accountId, int id);
    void Delete(string customerId, int accountId, int id);
}
