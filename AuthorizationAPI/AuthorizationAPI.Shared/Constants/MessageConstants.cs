namespace AuthorizationAPI.Shared.Constants
{
    public static class MessageConstants
    {
        public static string SuccessMessage { get; } = "Success!";
        public static string FailedMessage { get; } = "Failed!";
        public static string SuccessCreateMessage { get; } = "Successfully Created!";
        public static string FailedCreateMessage { get; } = "Creating Failed!";
        public static string SuccessDeleteMessage { get; } = "Successfully Deleted!";
        public static string FailedDeleteMessage { get; } = "Deleting Failed!";
        public static string SuccessUpdateMessage { get; } = "Successfully Updated!";
        public static string FailedUpdateMessage { get; } = "Updating Failed!";
        public static string NotFoundMessage { get; } = "Not Found!";
        public static string ForbiddenMessage { get; } = "Forbidden Action!";
        public static string CheckDBDataMessage { get; } = "Check DataBase Data!";
        public static string CheckCredsMessage { get; } = "Check credetials you have entered!";
        public static string EmailRegisteredMessage { get; } = "The Email is already registered!";
    }
}
