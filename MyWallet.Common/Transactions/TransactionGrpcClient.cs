using AutoMapper;
using MyWallet.Common.Models;
using static MyWallet.Common.TransactionGrpcService;

namespace MyWallet.Common.Transactions;

public interface ITransactionGrpcClient
{
    Task<ICollection<TransactionDto>> GetAllAsync(string customerId, int? accountId = null, int? walletId = null);
    Task ReverseAsync(int transactionId);
}

internal class TransactionGrpcClient : ITransactionGrpcClient
{
    private readonly TransactionGrpcServiceClient _transactionGrpcServiceClient;
    private readonly IMapper _mapper;

    public TransactionGrpcClient(
        TransactionGrpcServiceClient transactionGrpcServiceClient,
        IMapper mapper)
    {
        _transactionGrpcServiceClient = transactionGrpcServiceClient;
        _mapper = mapper;
    }

    public async Task<ICollection<TransactionDto>> GetAllAsync(string customerId, int? accountId = null, int? walletId = null)
    {
        var response = await _transactionGrpcServiceClient.GetAllAsync(new GetAllRequest
        {
            CustomerId = customerId,
            AccountId = accountId,
            WalletId = walletId
        });
        return _mapper.Map<ICollection<TransactionDto>>(response.Transactions);
    }

    public async Task ReverseAsync(int transactionId)
    {
        await _transactionGrpcServiceClient.ReverseAsync(new ReverseRequest
        {
            TransactionId = transactionId
        });
    }
}
