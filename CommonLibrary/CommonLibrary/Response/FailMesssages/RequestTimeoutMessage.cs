using System.ComponentModel;

namespace CommonLibrary.Response.FailMesssages
{
    public class RequestTimeoutMessage : FailMessage
    {
        [DefaultValue(408)]
        public int StatusCode { get; }
        public string ExceptionType { get; }
        public string ExceptionMessage { get; }
        public RequestTimeoutMessage(
            string exceptionType, 
            string exceptionMessage) : base("Request Timeout!")
        {
            ExceptionType = exceptionType;
            ExceptionMessage = exceptionMessage;
        }
    }
}
