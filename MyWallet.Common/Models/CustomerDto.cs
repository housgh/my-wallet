namespace MyWallet.Common.Models;

public class CustomerDto : BaseEntityDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
