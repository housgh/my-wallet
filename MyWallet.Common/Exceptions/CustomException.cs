using System.Net;

namespace MyWallet.Common.Exceptions;

public abstract class CustomException(string message, HttpStatusCode? statusCode = null) : Exception(message)
{
    public HttpStatusCode StatusCode { get; set; } = statusCode ?? HttpStatusCode.BadRequest;
}
