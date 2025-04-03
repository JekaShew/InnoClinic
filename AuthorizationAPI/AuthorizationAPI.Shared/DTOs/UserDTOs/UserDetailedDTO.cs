using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using AuthorizationAPI.Shared.DTOs.UserStatusDTOs;

namespace AuthorizationAPI.Shared.DTOs.UserDTOs;

public class UserDetailedDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SecondName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public Guid RoleId { get; set; }
    public Guid UserStatusId { get; set; }
    public RoleInfoDTO Role { get; set; }
    public UserStatusInfoDTO UserStatus { get; set; }
}
