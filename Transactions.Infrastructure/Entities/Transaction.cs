using MyWallet.Common.Enums;
using MyWallet.Common.Models;

namespace Transactions.Infrastructure.Entities;

internal class Transaction : BaseEntity
{
    public Guid TraceId { get; set; }
    public string? SourceCustomerId { get; set; }
    public int? SourceAccountId { get; set; }
    public int? SourceWalletId { get; set; }
    public string? DestinationCustomerId { get; set; }
    public int? DestinationAccountId { get; set; }
    public int? DestinationWalletId { get; set; }
    public TransactionTypeEnum Type { get; set; }
    public double Amount { get; set; }
    public bool IsReverse { get; set; }
    public CurrencyEnum Currency { get; set; }
}
