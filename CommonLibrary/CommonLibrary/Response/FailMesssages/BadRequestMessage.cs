using System.ComponentModel;

namespace CommonLibrary.Response.FailMesssages
{
    public class BadRequestMessage : FailMessage
    {
        [DefaultValue(400)]
        public int StatusCode { get; }
        public BadRequestMessage(string message) : base(message)
        { }
    }
}
