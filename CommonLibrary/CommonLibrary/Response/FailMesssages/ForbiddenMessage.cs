using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CommonLibrary.Response.FailMesssages
{
    public class ForbiddenMessage : FailMessage
    {
        //[DefaultValue(403)]
        [JsonIgnore]
        public const int StatusCode = StatusCodes.Status403Forbidden;
        public string Message { get; }
        public ForbiddenMessage(string message) : base(message, StatusCode)
        {
            Message = message;
        }
    }
}
