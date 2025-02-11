using System.ComponentModel;

namespace CommonLibrary.Response.SuccessMessages
{
    public class SuccessOnDeleteMessage : SuccessMessage
    {
        [DefaultValue(204)]
        public int StatusCode { get; }
        public SuccessOnDeleteMessage(string message) : base(message)
        { }
    }
}
