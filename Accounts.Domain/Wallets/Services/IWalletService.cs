using Accounts.Domain.Wallets.Dtos;
using Accounts.Domain.Wallets.Models;
using MyWallet.Common.Models;

namespace Accounts.Domain.Wallets.Services;

public interface IWalletService
{
    Task<WalletDto> GetAsync(string customerId, int accountId, int id);
    Task<ICollection<WalletDto>> GetAllAsync(string customerId, int accountId);
    Task<WalletDto> AddAsync(WalletDto walletDto);
    Task DebitAsync(string customerId, int accountId, int walletId, Money amount);
    Task CreditAsync(string customerId, int accountId, int walletId, Money amount);
    Task UpdateActive(string customerId, int accountId, int walletId);
    Task TransferAsync(TransferRequest request);
    Task DeleteAsync(string customerId, int accountId, int id);
    Task ReverseAsync(int transactionId);
}
