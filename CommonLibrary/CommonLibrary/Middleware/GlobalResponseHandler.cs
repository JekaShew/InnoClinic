using Microsoft.AspNetCore.Http;

namespace InnoClinic.CommonLibrary.Middleware
{
    public class GlobalResponseHandler(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            //try
            //{
            //    await next(httpContext);

            //    if (httpContext.Response.StatusCode == StatusCodes.Status429TooManyRequests)
            //    {
            //        var title = "Alert!";
            //        var message = "Too many requests were made!";
            //        var statusCode = (int)StatusCodes.Status429TooManyRequests;
                    
            //        await ModifyResponse(httpContext, title, message, statusCode);
            //    }

            //    if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
            //    {
            //        var title = "Warning!";
            //        var message = "You are UnAuthorized!";
            //        var statusCode = (int)StatusCodes.Status401Unauthorized;
                    
            //        await ModifyResponse(httpContext, title, message, statusCode);
            //    }

            //    if (httpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
            //    {
            //        var title = "Warning!";
            //        var message = "The resource is forbidden for you!";
            //        var statusCode = (int)StatusCodes.Status403Forbidden;
                    
            //        await ModifyResponse(httpContext, title, message, statusCode);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }

        private static async Task ModifyResponse(
                    HttpContext httpContext, 
                    string title, 
                    string message, 
                    int statusCode)
        {
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                Title = title,
                Message = message,
                StatusCode = statusCode
            });
        }
    }
}

