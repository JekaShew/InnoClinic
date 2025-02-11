namespace CommonLibrary.Response.SuccessMessages
{
    public class SuccessMessage
    {
        public string Message { get; }
        public SuccessMessage(string message)
        {
            Message = message;
        }
    }
    public class SuccessMessage<T> : SuccessMessage
    {
        public T? Value { get; }
        public SuccessMessage(string message, T? value) : base(message)
        {
            Value = value;
        }
    }
}
