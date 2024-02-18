using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWallet.Common.Exceptions;
using System.Text.Json;

namespace MyWallet.Common.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            var response = context.Response;
            response.ContentType = "application/json";
            ProblemDetails exceptionResponse;
            if (exception is CustomException customException)
            {
                exceptionResponse = new ProblemDetails()
                {
                    Status = (int) customException.StatusCode,
                    Detail = customException.Message,
                    Type = customException.GetType().Name
                };
            }
            else
            {
                exceptionResponse = new ProblemDetails()
                {
                    Status = 500,
                    Detail = "An Unexpected error has occured.",
                    Title = nameof(Exception)
                };
            }

            response.StatusCode = exceptionResponse.Status.Value;
            await response.WriteAsync(JsonSerializer.Serialize(exceptionResponse));
        }
    }
}
