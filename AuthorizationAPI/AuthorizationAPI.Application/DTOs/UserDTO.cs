namespace AuthorizationAPI.Application.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }

        public Guid RoleId { get; set; }
        public Guid UserStatusId { get; set; }
        public ParameterDTO Role { get; set; }
        public ParameterDTO UserStatus { get; set; } 
    }
}
