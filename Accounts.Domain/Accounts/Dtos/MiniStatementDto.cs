using Accounts.Domain.Wallets.Dtos;
using MyWallet.Common.Models;
using System.Text.Json;

namespace Accounts.Domain.Accounts.Dtos;

public class MiniStatementDto
{
    public required string CustomerId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public List<DetailedAccount> Accounts { get; set; } = new();
}

public class DetailedAccount : AccountDto
{
    public List<DetailedWallet> Wallets { get; set; } = new();

    public static DetailedAccount Create(AccountDto account)
    {
        var serializedWallet = JsonSerializer.Serialize(account);
        return JsonSerializer.Deserialize<DetailedAccount>(serializedWallet)!;
    }
}

public class DetailedWallet : WalletDto
{
    public List<TransactionDto> Transactions { get; set; } = new();

    public static DetailedWallet Create(WalletDto wallet)
    {
        var serializedWallet = JsonSerializer.Serialize(wallet);
        return JsonSerializer.Deserialize<DetailedWallet>(serializedWallet)!;
    }
}



