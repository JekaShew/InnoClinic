using System.ComponentModel;

namespace CommonLibrary.Response.FailMesssages
{
    public class ValidationErrorMessage //: FailMessage
    {
        [DefaultValue(422)]
        public int StatusCode { get; }
        public string[] Errors { get; }
        public ValidationErrorMessage(string Message, string[] errors) //: base(Message)
        {
            Errors = errors;
        }
    }
}
