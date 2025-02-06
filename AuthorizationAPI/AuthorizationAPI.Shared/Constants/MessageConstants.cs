namespace AuthorizationAPI.Shared.Constants;

public static class MessageConstants
{
    public static KeyValuePair<string, string> SuccessMessage { get; } = 
        new KeyValuePair<string, string>("200Base", "Success!");
    public static KeyValuePair<string, string> FailedMessage { get; } = 
        new KeyValuePair<string, string>("400Base", "Failed!");
    public static KeyValuePair<string, string> SuccessCreateMessage { get; } = 
        new KeyValuePair<string, string>("201Create", "Successfully Created!");
    public static KeyValuePair<string, string> FailedCreateMessage { get; } = 
        new KeyValuePair<string, string>("400Create", "Creating Failed!");
    public static KeyValuePair<string, string> SuccessDeleteMessage { get; } = 
        new KeyValuePair<string, string>("204Delete", "Successfully Deleted!");
    public static KeyValuePair<string, string> FailedDeleteMessage { get; } = 
        new KeyValuePair<string, string>("400Delete", "Deleting Failed!");
    public static KeyValuePair<string, string> SuccessUpdateMessage { get; } = 
        new KeyValuePair<string, string>("200Update", "Successfully Updated!");
    public static KeyValuePair<string, string> FailedUpdateMessage { get; } = 
        new KeyValuePair<string, string>("400Update", "Updating Failed!");
    public static KeyValuePair<string, string> NotFoundMessage { get; } = 
        new KeyValuePair<string, string>("404", "Not Found!");
    public static KeyValuePair<string, string> ForbiddenMessage { get; } = 
        new KeyValuePair<string, string>("403", "Forbidden Action!");
    public static KeyValuePair<string, string> CheckDBDataMessage { get; } = 
        new KeyValuePair<string, string>("400CheckDB", "Check DataBase Data!");
    public static KeyValuePair<string, string> CheckCredsMessage { get; } =
        new KeyValuePair<string, string>("400CheckCreds", "Check credetials you have entered!");
    public static KeyValuePair<string, string> EmailRegisteredMessage { get; } = 
        new KeyValuePair<string, string>("400EmailRegistered", "The Email is already registered!");
}
