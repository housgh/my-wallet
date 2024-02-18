using System.Net;

namespace MyWallet.Common.Exceptions;

public abstract class NotFoundException(string message) : CustomException(message, HttpStatusCode.NotFound)
{
}
