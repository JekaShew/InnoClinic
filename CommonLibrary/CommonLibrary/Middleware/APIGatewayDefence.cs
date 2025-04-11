using CommonLibrary.Constants;
using Microsoft.AspNetCore.Http;

namespace CommonLibrary.Middlewarel;
public class APIGatewayDefence(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request is coming API Gateway
        var apiGatewayHeader = context.Request.Headers["API-Gateway"];
        if (apiGatewayHeader.FirstOrDefault() is null || !apiGatewayHeader.FirstOrDefault().Equals(APIGatewaySignature.ApiGateWaySignature))
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Sorry, Service is unavailable! Try to use our Client UI to access the resource!");
            return;
        }
        
        await next(context);
    }
}
