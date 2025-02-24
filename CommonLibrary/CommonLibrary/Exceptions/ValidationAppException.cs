namespace InnoClinic.CommonLibrary.Exceptions;

public class ValidationAppException : Exception
{
    public string[] Errors { get;}
    public ValidationAppException(string[] errors)
        : base("One or more validation errors occured!")
    {
        this.Errors = errors;
    }
}
