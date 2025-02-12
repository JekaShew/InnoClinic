using System.ComponentModel;

namespace CommonLibrary.Response.FailMesssages
{
    public class NotFoundMessage //: FailMessage
    {
        [DefaultValue(404)]
        public int StatusCode { get; }
        public NotFoundMessage(string message) //: base(message)
        { }
    }
}
