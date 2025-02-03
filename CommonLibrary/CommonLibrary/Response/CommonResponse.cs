namespace InnoClinic.CommonLibrary.Response
{
    public class CommonResponse
    {
        public bool Flag { get; }
        public string Message { get; }
        public CommonResponse(bool flag = false, string message = null!)
        {
            Flag = flag;
            Message = message;
        }
    }

    public class CommonResponse<T> : CommonResponse
    {
        public T Value { get; }

        public CommonResponse(bool flag = false, string message = null!, T value = default!)
            : base(flag, message)
        {
            Value = value;
        }
    }
}
