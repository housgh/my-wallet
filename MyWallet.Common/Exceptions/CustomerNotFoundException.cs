namespace MyWallet.Common.Exceptions;

public class CustomerNotFoundException(string id) : NotFoundException($"Customer with id '{id}' does not exist")
{
}
