namespace AuthorizationAPI.Shared.DTOs
{
    public class AuthorizationInfoDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string SecretPhraseHash { get; set; }
        public string SecurityStamp { get; set; }
    }
}
