namespace AuthorizationAPI.Shared.DTOs.UserDTOs
{
    public class UserForUpdateByAdministratorDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public Guid RoleId { get; set; }
        public Guid UserStatusId { get; set; }
    }
}
