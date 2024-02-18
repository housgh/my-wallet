using MyWallet.Common.Enums;

namespace MyWallet.Common.Models;

public record Money(double Amount, CurrencyEnum Currency);
