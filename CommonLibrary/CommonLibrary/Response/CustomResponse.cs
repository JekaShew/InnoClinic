namespace InnoClinic.CommonLibrary.Response
{
    public class CustomResponse
    {
        public bool Flag { get; }
        public string Message { get; }
        public CustomResponse(bool flag = false, string message = null!)
        {
            Flag = flag;
            Message = message;
        }
    }

    public class CustomResponse<T> : CustomResponse
    {
        public T Value { get; }

        public CustomResponse(bool flag = false, string message = null!, T value = default!)
            : base(flag, message)
        {
            Value = value;
        }
    }
}
