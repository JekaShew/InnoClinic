namespace InnoClinic.CommonLibrary.Response;

public class ResponseMessage
{
    public bool IsComplited => string.IsNullOrWhiteSpace(ErrorMessage);
    public string ErrorMessage { get; }
    public int StatusCode { get; }

    // when Failed
    public ResponseMessage(string errorMessage, int statusCode)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
    }
    // when Succeeded
    public ResponseMessage()
    {
    }
}

public class ResponseMessage<T> : ResponseMessage
{
    public T Value { get; }

    // when Failed
    public ResponseMessage(string message, int statusCode)
        : base(message, statusCode)
    {
        
    }

    // when Succeeded
    public ResponseMessage(T value = default!)
    : base()
    {
        Value = value;
    }
}


//public class ResponseMessage
//{
//    public bool Flag { get; }
//    public KeyValuePair<string,string> Message { get; }

//    public ResponseMessage(KeyValuePair<string, string> message, bool flag = false)
//    {
//        Flag = flag;
//        Message = message;
//    }
//}

//public class ResponseMessage<T> : ResponseMessage
//{
//    public T Value { get; }

//    public ResponseMessage(KeyValuePair<string, string> message, bool flag = false,  T value = default!)
//        : base(message, flag )
//    {
//        Value = value;
//    }
//}