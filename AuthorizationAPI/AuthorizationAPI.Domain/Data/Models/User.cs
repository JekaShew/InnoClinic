namespace AuthorizationAPI.Domain.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string SecretPhraseHash { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Guid UserStatusId { get; set; }
        public UserStatus UserStatus { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }

    }
}
