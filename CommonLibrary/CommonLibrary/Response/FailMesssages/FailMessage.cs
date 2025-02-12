using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace CommonLibrary.Response.FailMesssages;

public class FailMessage : IActionResult
{
    
    public int StatusCode { get; }
    public string Message { get; }
    [JsonIgnore]
    public string[]? InnerErrors { get; }
    [JsonIgnore]
    private object Details { get; set; }
    public FailMessage(string message, int statusCode = 400, string[] innerErrors = null)
    {
        StatusCode = statusCode;
        Message = message;
        InnerErrors = innerErrors;

        if(innerErrors is not null)
        Details = new
        {
            StatusCode = statusCode,
            Message = message,
            InnerErrors = innerErrors
        };

        Details = new
        {
            StatusCode = statusCode,
            Message = message
        };
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        response.StatusCode = StatusCode;
        response.ContentType = "plain/text";        

        await response.WriteAsJsonAsync(Details);
    }
}
