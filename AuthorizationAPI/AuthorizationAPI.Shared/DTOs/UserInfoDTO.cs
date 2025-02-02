namespace AuthorizationAPI.Shared.DTOs
{
    public class UserInfoDTO
    {
        public Guid Id { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
    }
}
