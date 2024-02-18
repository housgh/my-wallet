using MyWallet.Common.Enums;

namespace MyWallet.Common.Models;

public class TransactionDto : BaseEntityDto
{
    public string? SourceCustomerId { get; set; }
    public int? SourceAccountId { get; set; }
    public int? SourceWalletId { get; set; }
    public string? DestinationCustomerId { get; set; }
    public int? DestinationAccountId { get; set; }
    public int? DestinationWalletId { get; set; }
    public Guid TraceId { get; set; }
    public TransactionTypeEnum Type { get; set; }
    public double Amount { get; set; }
    public CurrencyEnum Currency { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public bool IsReverse { get; set; }
}
