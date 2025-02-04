namespace AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs
{
    public class UserLoggedInInfoDTO
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string LoggedInDateTime { get; set; }
    }
}
