using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MyWallet.Common;
using Transactions.Domain.Transaction.Services;

namespace Transactions.Grpc.Services
{
    public class TransactionService : TransactionGrpcService.TransactionGrpcServiceBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionService(
            ITransactionService transactionService,
            IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        public override async Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
        {
            var transactions = await _transactionService.GetAllAsync(request.CustomerId, request.AccountId, request.WalletId);
            return new GetAllResponse
            {
                Transactions = { _mapper.Map<ICollection<Transaction>>(transactions.Transactions) },
                TotalBalance = transactions.TotalBalance
            };
        }

        public override async Task<Empty> Reverse(ReverseRequest request, ServerCallContext context)
        {
            await _transactionService.ReverseAsync(request.TransactionId);
            return new Empty();
        }
    }
}
