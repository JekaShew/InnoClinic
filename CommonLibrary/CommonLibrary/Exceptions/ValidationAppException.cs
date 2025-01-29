namespace InnoShop.CommonLibrary.Exceptions
{
    public class ValidationAppException : Exception
    {
        public IReadOnlyDictionary<string, string[]> Errors { get;}
        public ValidationAppException(IReadOnlyDictionary<string, string[]> errors)
            : base("One or more validation errors occured!")
        {
            this.Errors = errors;
        }
    }
}
