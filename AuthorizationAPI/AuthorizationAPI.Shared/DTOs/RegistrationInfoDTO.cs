namespace AuthorizationAPI.Shared.DTOs
{
    public class RegistrationInfoDTO
    {
        public Guid Id { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SecretPhrase { get; set; }
    }
}
