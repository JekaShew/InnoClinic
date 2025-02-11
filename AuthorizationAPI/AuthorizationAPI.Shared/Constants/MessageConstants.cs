namespace AuthorizationAPI.Shared.Constants;

public static class MessageConstants
{
    public const string Base200 = "200Base";
    public const string Base400 = "400Base";
    public const string Create201 = "201Create";
    public const string Create400 = "400Create";
    public const string Delete204 = "204Delete";
    public const string Delete400 = "400Delete";
    public const string Update200 = "200Update";
    public const string Update400 = "400Update";
    public const string Base404 = "404";
    public const string Base403 = "403";
    public const string CheckDB400 = "400CheckDB";
    public const string CheckCreds400 = "400CheckCreds";
    public const string EmailRegistered400 = "400EmailRegistered";
    public static KeyValuePair<string, string> SuccessMessage { get; } = 
        new KeyValuePair<string, string>(Base200, "Success!");
    public static KeyValuePair<string, string> FailedMessage { get; } = 
        new KeyValuePair<string, string>(Base400, "Failed!");
    public static KeyValuePair<string, string> SuccessCreateMessage { get; } = 
        new KeyValuePair<string, string>(Create201, "Successfully Created!");
    public static KeyValuePair<string, string> FailedCreateMessage { get; } = 
        new KeyValuePair<string, string>(Create400, "Creating Failed!");
    public static KeyValuePair<string, string> SuccessDeleteMessage { get; } = 
        new KeyValuePair<string, string>(Delete204, "Successfully Deleted!");
    public static KeyValuePair<string, string> FailedDeleteMessage { get; } = 
        new KeyValuePair<string, string>(Delete400, "Deleting Failed!");
    public static KeyValuePair<string, string> SuccessUpdateMessage { get; } = 
        new KeyValuePair<string, string>(Update200, "Successfully Updated!");
    public static KeyValuePair<string, string> FailedUpdateMessage { get; } = 
        new KeyValuePair<string, string>(Update400, "Updating Failed!");
    public static KeyValuePair<string, string> NotFoundMessage { get; } = 
        new KeyValuePair<string, string>(Base404, "Not Found!");
    public static KeyValuePair<string, string> ForbiddenMessage { get; } = 
        new KeyValuePair<string, string>(Base403, "Forbidden Action!");
    public static KeyValuePair<string, string> CheckDBDataMessage { get; } = 
        new KeyValuePair<string, string>(CheckDB400, "Check DataBase Data!");
    public static KeyValuePair<string, string> CheckCredsMessage { get; } =
        new KeyValuePair<string, string>(CheckCreds400, "Check credetials you have entered!");
    public static KeyValuePair<string, string> EmailRegisteredMessage { get; } = 
        new KeyValuePair<string, string>(EmailRegistered400, "The Error occured on the server's side!");
}
