using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace InnoClinic.CommonLibrary.Response
{
    //public enum ResponseTypes {Ok, BadRequest, Forbidden, NotFound }
    public class ResponseMessage
    {
        public bool Flag { get; }
        public KeyValuePair<string,string> Message { get; } = new KeyValuePair<string,string>();
        //public ResponseTypes ResponseType { get; }

        public ResponseMessage(KeyValuePair<string, string> message, bool flag = false)
        {
            Flag = flag;
            Message = message;
        }
    }

    public class ResponseMessage<T> : ResponseMessage
    {
        public T Value { get; }

        public ResponseMessage(KeyValuePair<string, string> message, bool flag = false,  T value = default!)
            : base(message, flag )
        {
            Value = value;
        }
    }
}
