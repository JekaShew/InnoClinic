namespace AuthorizationAPI.Shared.DTOs.UserDTOs;

public class UserInfoDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SecondName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
}
