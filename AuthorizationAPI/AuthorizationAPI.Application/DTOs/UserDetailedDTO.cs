namespace AuthorizationAPI.Application.DTOs
{
    public class UserDetailedDTO
    {
        public Guid Id { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string SecretPhraseHash { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Guid UserStatusId { get; set; }
        public Guid RoleId { get; set; }
    }
}
