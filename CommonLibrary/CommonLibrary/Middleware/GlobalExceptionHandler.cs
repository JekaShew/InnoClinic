using InnoShop.CommonLibrary.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace InnoShop.CommonLibrary.Middleware
{
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
                httpContext.Response.StatusCode = (int)StatusCodes.Status408RequestTimeout;

                var problemDetailsContext = new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails =
                    {
                       Title = "Alert!",
                        Detail = "Request timeout! Please, try again!",
                        Type = exception.GetType().Name,
                        Status = (int)StatusCodes.Status408RequestTimeout
                    },
                    Exception = exception
                };

                return await ModifyExceptopnResponse(problemDetailsContext);
            }

            if(exception is ValidationAppException validationException)
            {
                httpContext.Response.StatusCode = (int)StatusCodes.Status422UnprocessableEntity;

                await httpContext.Response.WriteAsJsonAsync(
                    new
                    {
                        Type = exception.GetType().Name,
                        Title = exception.Message,
                        Errors = validationException.Errors,
                        StatusCode = (int)StatusCodes.Status422UnprocessableEntity
                    });
                return true;
            }

            return await ModifyExceptopnResponse(
                new ProblemDetailsContext 
                {
                    HttpContext = httpContext,
                    ProblemDetails = 
                    {
                      Title = "Error!",
                      Detail = "Oops! Something went wrong! Internal server error occured! Please, try again!",
                      Type = exception.GetType().Name,
                      Status = (int)StatusCodes.Status500InternalServerError
                    },
                    Exception = exception
                });
        }

        private async Task<bool> ModifyExceptopnResponse(ProblemDetailsContext problemDetailsContext)
        {
            return await this._problemDetailsService.TryWriteAsync(problemDetailsContext);
        }
    }  
}
