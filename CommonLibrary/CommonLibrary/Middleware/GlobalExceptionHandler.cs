using InnoClinic.CommonLibrary.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using CommonLibrary.Response;

namespace InnoClinic.CommonLibrary.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{

    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
    { }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";

        if(exception is TaskCanceledException || exception is TimeoutException) 
        {
            return await ModifyExceptionResponse(
                httpContext,
                new FailMessage(exception.Message, 408));
        }

        if(exception is ValidationAppException validationException)
        {
            return await ModifyExceptionResponse(
                httpContext,
                new FailMessage(validationException.Message, 422, validationException.Errors));
        }

        return await ModifyExceptionResponse( 
            httpContext,
            new FailMessage(exception.Message, 500));
    }

    private async Task<bool> ModifyExceptionResponse(HttpContext httpContext, FailMessage failMessage)
    {
        var response = httpContext.Response;
        response.StatusCode = failMessage.StatusCode;
        response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(failMessage.Details);

        return true;
    }
}  
