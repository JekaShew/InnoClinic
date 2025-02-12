using System.ComponentModel;

namespace CommonLibrary.Response.FailMesssages
{
    public class ServerErrorMessage //: FailMessage
    {
        [DefaultValue(500)]
        public int StatusCode { get; }
        public string ExceptionType { get; }
        public string ExceptionMessage { get; }
        public ServerErrorMessage(
            string exceptionType, 
            string exceptionMessage ) //: base("Internal Server Error!")
        {
            ExceptionType = exceptionType;
            ExceptionMessage = exceptionMessage;

        }
    }
}
