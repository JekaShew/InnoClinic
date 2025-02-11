using System.ComponentModel;

namespace CommonLibrary.Response.SuccessMessages
{
    public class SuccessOnCreateMessage : SuccessMessage
    {
        [DefaultValue(201)]
        public int StatusCode { get; }
        public SuccessOnCreateMessage(string message) : base(message)
        { }
    }
}
