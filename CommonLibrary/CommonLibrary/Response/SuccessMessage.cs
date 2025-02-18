using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace CommonLibrary.Response
{
    public class SuccessMessage : IActionResult
    {
        public int StatusCode { get; set; }
        public string Message { get; }
        [JsonIgnore]
        public object Details { get; set; }
        public SuccessMessage(string message, int statusCode = 200)
        {
            StatusCode = statusCode;
            Message = message;
            Details = new
            {
                StatusCode = statusCode,
                Message = message,
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
    public class SuccessMessage<T> : IActionResult
    {
        public string Message { get; }
        public object Details { get; set; }
        public SuccessMessage(string message, T? value)
        {
            Message = message;
            Details = new
            {
                Message = message,
                Value = value
            };
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = "plain/text";

            await response.WriteAsJsonAsync(Details);
        }
    }
}
