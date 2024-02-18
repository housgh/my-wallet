using MyWallet.Common.Models;

namespace Accounts.Domain.Wallets.Models;

public class TransferRequest
{
    public required string SourceCustomerId { get; set; }
    public int SourceAccountId { get; set; }
    public int SourceWalletId { get; set; }
    public required string DestinationCustomerId { get; set; }
    public int DestinationAccountId { get; set; }
    public int DestinationWalletId { get; set; }
    public required Money Amount { get; set; }
}
