using Accounts.Domain.Accounts.Dtos;
using Accounts.Domain.Accounts.Exceptions;
using Accounts.Domain.Accounts.Repositories;
using Accounts.Domain.Accounts.Services;
using Accounts.Domain.Wallets.Dtos;
using Accounts.Domain.Wallets.Repositories;
using MyWallet.Common.Customers;
using MyWallet.Common.Enums;
using MyWallet.Common.Models;
using MyWallet.Common.Transactions;
using MyWallet.Common.Exceptions;

namespace Accounts.Application.Services;

internal class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    private readonly ITransactionProducer _transactionProducer;
    private readonly ITransactionGrpcClient _transactionGrpcClient;
    private readonly IWalletRepository _walletRepository;
    private readonly ICustomerGrpcClient _customerGrpcClient;

    public AccountService(
        IAccountRepository repository,
        IWalletRepository walletRepository,
        ITransactionProducer transactionProducer,
        ITransactionGrpcClient transactionGrpcClient,
        ICustomerGrpcClient customerGrpcClient)
    {
        _repository = repository;
        _transactionProducer = transactionProducer;
        _transactionGrpcClient = transactionGrpcClient;
        _walletRepository = walletRepository;
        _customerGrpcClient = customerGrpcClient;
    }

    public async Task<AccountDto> GetAsync(string customerId, int id)
    {
        if(!await _customerGrpcClient.ExistsAsync(customerId))
            throw new CustomerNotFoundException(customerId);
        return await _repository.GetAsync(customerId, id) ?? 
            throw new AccountNotFoundException(id);
    }

    public async Task<ICollection<AccountDto>> GetAllAsync(string customerId)
    {
        if (!await _customerGrpcClient.ExistsAsync(customerId))
            throw new CustomerNotFoundException(customerId);
        return await _repository.GetAllAsync(customerId);
    }

    public async Task<AccountDto> AddAsync(ExtendedAccountDto extendedAccountDto)
    {
        if (!await _customerGrpcClient.ExistsAsync(extendedAccountDto.CustomerId))
            throw new CustomerNotFoundException(extendedAccountDto.CustomerId);
        var account = new AccountDto
        {
            CustomerId = extendedAccountDto.CustomerId,
            AccountType = extendedAccountDto.AccountType,
            Active = true,
        };
        var initialWallet = new WalletDto
        {
            Currency = extendedAccountDto.Currency,
            CreditLimit = extendedAccountDto.CreditLimit,
            Balance = extendedAccountDto.Balance,
            Active = true
        };
        await _repository.AddAsync(account, initialWallet);
        await _repository.SaveChangesAsync();

        if(initialWallet.Balance > 0)
        {
            _transactionProducer.SendMessage(new TransactionDto
            {
                Amount = extendedAccountDto.Balance,
                Currency = extendedAccountDto.Currency,
                DestinationCustomerId = extendedAccountDto.CustomerId,
                DestinationAccountId = account.Id,
                DestinationWalletId = initialWallet.Id,
                Type = TransactionTypeEnum.CashToWallet,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            });
        }

        return account;
    }

    public async Task<AccountDto> UpdateStatusAsync(string customerId, int accountId)
    {
        if (!await _customerGrpcClient.ExistsAsync(customerId))
            throw new CustomerNotFoundException(customerId);
        var account = await _repository.GetAsync(customerId, accountId) ??
            throw new AccountNotFoundException(accountId);
        account.Active = !account.Active;
        _repository.Update(account);
        await _repository.SaveChangesAsync();
        return account;
    }

    public async Task RemoveAsync(string customerId, int id)
    {
        if (!await _customerGrpcClient.ExistsAsync(customerId))
            throw new CustomerNotFoundException(customerId);
        if (!await _repository.ExistsAsync(customerId, id))
            throw new AccountNotFoundException(id);
        _repository.DeleteAsync(customerId, id);
        await _repository.SaveChangesAsync();
    }

    public async Task<MiniStatementDto> GetMiniStatementAsync(string customerId, int? accountId = null, int? walletId = null)
    {
        var customer = await _customerGrpcClient.GetAsync(customerId);

        var miniStatement = new MiniStatementDto
        {
            CustomerId = customerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName
        };

        var accounts = await _repository.GetAllAsync(customerId);
        var wallets = await _walletRepository.GetAllAsync(customerId, accountId);

        var transactions = await _transactionGrpcClient.GetAllAsync(customerId, accountId, walletId);

        foreach(var account in accounts)
        {
            var detailedAccount = DetailedAccount.Create(account);
            foreach(var wallet in wallets.Where(x => x.AccountId == account.Id))
            {
                var detailedWallet = DetailedWallet.Create(wallet);
                detailedWallet.Transactions.AddRange(transactions.Where(x => x.SourceWalletId == wallet.Id || x.DestinationWalletId == wallet.Id));
                detailedAccount.Wallets.Add(detailedWallet);
            }
            miniStatement.Accounts.Add(detailedAccount);
        }
        return miniStatement;
    }
}
