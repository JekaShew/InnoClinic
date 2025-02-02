namespace AuthorizationAPI.Shared.DTOs
{
    public class UserDetailedDTO
    {
        public Guid Id { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public Guid RoleId { get; set; }
        public Guid UserStatusId { get; set; }
        public ParameterDTO Role { get; set; }
        public ParameterDTO UserStatus { get; set; }
    }
}
