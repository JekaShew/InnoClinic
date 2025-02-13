namespace AuthorizationAPI.Shared.Constants;

public static class EmailTemplates
{
    public const string VerificationSubject = "Email Verification";
    public static KeyValuePair<string, string> VerificationTemplate { get; } 
        = new KeyValuePair<string, string> (VerificationSubject,"Dear User! \nYou should click the Link below to verify your Email! \nLink : ");
}
