namespace APIGateway.Middleware
{
    public class AttachAPIGatewaySignature(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Add a custom header to the request
            context.Request.Headers["API-Gateway"] = "Signed-By-SecretSignature123";

            await next(context);
        }
    }
}
