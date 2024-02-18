using Accounts.Domain.Accounts.Repositories;
using Accounts.Domain.Wallets.Dtos;
using Accounts.Domain.Wallets.Exceptions;
using Accounts.Domain.Wallets.Repositories;
using Accounts.Domain.Accounts.Exceptions;
using MyWallet.Common.Models;
using Accounts.Domain.Accounts.Enums;
using Accounts.Domain.Wallets.Models;
using Accounts.Domain.ConversionRates.Repositories;
using Accounts.Domain.Wallets.Services;
using MyWallet.Common.Transactions;
using MyWallet.Common.Enums;
using MyWallet.Common.Customers;
using MyWallet.Common.Exceptions;

namespace Accounts.Application.Services;

internal class WalletService : IWalletService
{
    private readonly IWalletRepository _repository;
    private readonly IAccountRepository _accountRepository;
    private readonly IConversionRateRepository _conversionRateRepository;
    private readonly ITransactionProducer _transactionProducer;
    private readonly ITransactionGrpcClient _transactionGrpcClient;
    private readonly ICustomerGrpcClient _customerGrpcClient;

    public WalletService(
        IWalletRepository repository,
        IAccountRepository accountRepository,
        IConversionRateRepository conversionRateRepository,
        ITransactionProducer transactionProducer,
        ITransactionGrpcClient transactionGrpcClient,
        ICustomerGrpcClient customerGrpcClient)
    {
        _repository = repository;
        _accountRepository = accountRepository;
        _conversionRateRepository = conversionRateRepository;
        _transactionProducer = transactionProducer;
        _transactionGrpcClient = transactionGrpcClient;
        _customerGrpcClient = customerGrpcClient;
    }

    public async Task<WalletDto> GetAsync(string customerId, int accountId, int id)
    {
        if (!await _customerGrpcClient.ExistsAsync(customerId))
            throw new CustomerNotFoundException(customerId);
        return await _repository.GetAsync(id) ?? 
            throw new WalletNotFoundException(id);
    }

    public async Task<ICollection<WalletDto>> GetAllAsync(string customerId, int accountId)
    {
        if (!await _customerGrpcClient.ExistsAsync(customerId))
            throw new CustomerNotFoundException(customerId);
        if (!await _accountRepository.ExistsAsync(customerId, accountId))
            throw new AccountNotFoundException(accountId);
        return await _repository.GetAllAsync(customerId, accountId);
    }

    public async Task<WalletDto> AddAsync(WalletDto walletDto)
    {
        var account = await _accountRepository.GetAsync(walletDto.AccountId);
        if (account is null)
            throw new AccountNotFoundException(walletDto.AccountId);
        if (await _repository.WalletExistsAsync(walletDto.AccountId, walletDto.Currency))
            throw new WalletAlreadyExistsException(walletDto.Currency);
        await _repository.AddAsync(walletDto);
        await _repository.SaveChangesAsync();

        if(walletDto.Balance > 0)
        {
            _transactionProducer.SendMessage(new TransactionDto
            {
                Amount = walletDto.Balance,
                Currency = walletDto.Currency,
                DestinationCustomerId = account.CustomerId,
                DestinationAccountId = walletDto.AccountId,
                DestinationWalletId = walletDto.Id,
                Type = TransactionTypeEnum.CashToWallet,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            });
        }

        return walletDto;
    }

    public async Task DebitAsync(string customerId, int accountId, int walletId, Money amount)
    {
        if (!await _customerGrpcClient.ExistsAsync(customerId))
            throw new CustomerNotFoundException(customerId);
        var wallet = await _repository.GetWalletWithAccount(customerId, accountId, walletId) ?? 
            throw new WalletNotFoundException(walletId);
        if (amount.Currency != wallet.Currency)
            throw new InvalidCurrencyException(wallet.Currency, amount.Currency);
        if(wallet.AccountType == AccountTypeEnum.Credit)
        {
            if (wallet.Balance + amount.Amount > wallet.CreditLimit)
                throw new CreditLimitExceededException(amount);
            wallet.Balance += amount.Amount;
        }

        if(wallet.AccountType == AccountTypeEnum.Debit)
        {
            if (wallet.Balance - amount.Amount < 0)
                throw new InsufficientFundException(amount);
            wallet.Balance -= amount.Amount;
        }

        _transactionProducer.SendMessage(new TransactionDto
        {
            Amount = amount.Amount,
            Currency = amount.Currency,
            SourceCustomerId = wallet.CustomerId,
            SourceAccountId = accountId,
            SourceWalletId = walletId,
            Type = TransactionTypeEnum.CashToWallet,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        });

        _repository.Update(wallet);
        await _repository.SaveChangesAsync();
    }

    public async Task CreditAsync(string customerId, int accountId, int walletId, Money amount)
    {
        if (!await _customerGrpcClient.ExistsAsync(customerId))
            throw new CustomerNotFoundException(customerId);
        var wallet = await _repository.GetWalletWithAccount(customerId, accountId, walletId) ??
           throw new WalletNotFoundException(walletId);
        if (wallet.AccountType == AccountTypeEnum.Credit)
            throw new InvalidTransactionException();
        if (amount.Currency != wallet.Currency)
            throw new InvalidCurrencyException(wallet.Currency, amount.Currency);
        wallet.Balance += amount.Amount;

        _transactionProducer.SendMessage(new TransactionDto
        {
            Amount = amount.Amount,
            Currency = amount.Currency,
            DestinationCustomerId = wallet.CustomerId,
            DestinationAccountId = accountId,
            DestinationWalletId = walletId,
            Type = TransactionTypeEnum.WalletToCash,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        });


        _repository.Update(wallet);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateActive(string customerId, int accountId, int walletId)
    {
        if (!await _customerGrpcClient.ExistsAsync(customerId))
            throw new CustomerNotFoundException(customerId);
        var wallet = await _repository.GetWalletWithAccount(customerId, accountId, walletId) ??
           throw new WalletNotFoundException(walletId);
        wallet.Active = !wallet.Active;
        _repository.Update(wallet);
        await _repository.SaveChangesAsync();
    }

    public async Task TransferAsync(TransferRequest request)
    {
        if (!await _customerGrpcClient.ExistsAsync(request.SourceCustomerId))
            throw new CustomerNotFoundException(request.SourceCustomerId);

        if (!await _customerGrpcClient.ExistsAsync(request.DestinationCustomerId))
            throw new CustomerNotFoundException(request.DestinationCustomerId);

        var sourceWallet = await _repository.GetWalletWithAccount(request.SourceCustomerId, request.SourceAccountId, request.SourceWalletId) ??
           throw new WalletNotFoundException(request.SourceWalletId);

        var destinationWallet = await _repository.GetWalletWithAccount(request.DestinationCustomerId, request.DestinationAccountId, request.DestinationWalletId) ??
           throw new WalletNotFoundException(request.DestinationWalletId);

        if(destinationWallet.AccountType == AccountTypeEnum.Credit)
            throw new InvalidTransactionException();

        if (request.Amount.Currency != sourceWallet.Currency)
            throw new InvalidCurrencyException(sourceWallet.Currency, request.Amount.Currency);

        if (sourceWallet.Balance - request.Amount.Amount < 0)
            throw new InsufficientFundException(request.Amount);

        sourceWallet.Balance -= request.Amount.Amount;

        if(destinationWallet.Currency != request.Amount.Currency)
        {
            var conversionRate = await _conversionRateRepository.GetAsync(request.Amount.Currency, destinationWallet.Currency);
            destinationWallet.Balance += request.Amount.Amount * conversionRate.Rate;
        }

        _transactionProducer.SendMessage(new TransactionDto
        {
            Amount = request.Amount.Amount,
            Currency = request.Amount.Currency,
            DestinationCustomerId = request.DestinationCustomerId,
            DestinationAccountId = request.DestinationAccountId,
            DestinationWalletId = request.DestinationWalletId,
            SourceCustomerId = request.SourceCustomerId,
            SourceAccountId = request.SourceAccountId,
            SourceWalletId = request.SourceWalletId,
            Type = request.SourceAccountId == request.DestinationAccountId ? TransactionTypeEnum.WalletToWalletInternal : TransactionTypeEnum.WalletToWalletExternal,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        });

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(string customerId, int accountId, int id)
    {
        if (!await _repository.ExistsAsync(customerId, accountId, id))
            throw new AccountNotFoundException(id);
        _repository.Delete(customerId, accountId, id);
        await _repository.SaveChangesAsync();
    }

    public async Task ReverseAsync(int transactionId)
    {
        await _transactionGrpcClient.ReverseAsync(transactionId);
    }
}
