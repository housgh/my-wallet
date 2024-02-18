using MyWallet.Common.Models;

namespace Users.Infrastructure.Entities;

internal class Customer : BaseEntity
{
    public string CustomerId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
