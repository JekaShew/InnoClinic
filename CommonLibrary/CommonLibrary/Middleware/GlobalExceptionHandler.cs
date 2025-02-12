using CommonLibrary.Response.FailMesssages;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System;

namespace InnoClinic.CommonLibrary.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";

        if(exception is TaskCanceledException || exception is TimeoutException) 
        {
            //httpContext.Response.StatusCode = (int)StatusCodes.Status408RequestTimeout;
            return await ModifyExceptionResponse(
                httpContext,
                new FailMessage(exception.Message, 408 ));
               
                //new RequestTimeoutMessage(
                //    exception.GetType().Name,
                //    exception.Message
                //));
            //var problemDetailsContext = new ProblemDetailsContext
            //{
            //    HttpContext = httpContext,
            //    ProblemDetails =
            //    {
            //        Title = "Alert!",
            //        Detail = "Request timeout! Please, try again!",
            //        Type = exception.GetType().Name,
            //        Status = (int)StatusCodes.Status408RequestTimeout
            //    },
            //    Exception = exception
            //};
        }

        if(exception is ValidationAppException validationException)
        {
            //httpContext.Response.StatusCode = (int)StatusCodes.Status422UnprocessableEntity;
            await ModifyExceptionResponse(
                httpContext,
                new FailMessage(validationException.Message, 500, validationException.Errors));
            //new ValidationErrorMessage(
            //        exception.Message,
            //        validationException.Errors
            //    ));

            return true;
        }

        return await ModifyExceptionResponse( 
            httpContext,
            new FailMessage(exception.Message, 500));
        //new ServerErrorMessage(
        //        exception.GetType().Name,
        //        exception.Message
        //    ));

        //new ProblemDetailsContext 
        //    {
        //        HttpContext = httpContext,
        //        ProblemDetails = 
        //        {
        //          Title = "Error!",
        //          Detail = "Oops! Something went wrong! Internal server error occured! Please, try again!",
        //          Type = exception.GetType().Name,
        //          Status = (int)StatusCodes.Status500InternalServerError
        //        },
        //        Exception = exception
        //    });
    }

    private async Task<bool> ModifyExceptionResponse(HttpContext httpContext, FailMessage failMessage)
    {
        await httpContext.Response.WriteAsJsonAsync(failMessage);

        return true;
    }
}  
