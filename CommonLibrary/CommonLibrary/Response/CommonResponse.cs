using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace InnoClinic.CommonLibrary.Response
{
    public enum ResponseTypes {Ok, BadRequest, Forbidden, NotFound }
    public class CommonResponse
    {
        public bool Flag { get; }
        public string Message { get; }
        public ResponseTypes ResponseType { get; }
        
        public CommonResponse(ResponseTypes responseType, bool flag = false, string message = null!)
        {
            Flag = flag;
            ResponseType = responseType;
            Message = message;
        }
    }

    public class CommonResponse<T> : CommonResponse
    {
        public T Value { get; }

        public CommonResponse(ResponseTypes responseType, bool flag = false, string message = null!, T value = default!)
            : base(responseType, flag, message)
        {
            Value = value;
        }
    }
}
