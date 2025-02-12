namespace AuthorizationAPI.Shared.DTOs.UserDTOs;

public class EmailSecretPhraseNewPasswordDTO
{
    public string Email { get; set; }
    public string SecretPhrase { get; set; }
    public string NewPassword { get; set; }
}
