using System.ComponentModel;

namespace CommonLibrary.Response.FailMesssages
{
    public class ForbiddenMessage : FailMessage
    {
        [DefaultValue(403)]
        public int StatusCode { get; }
        public ForbiddenMessage(string message): base(message)
        { }
    }
}
