using AuthorizationAPI.Shared.DTOs.UserDTOs;

namespace AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs;

public class UserLoggedInInfoDTO
{
    public Guid UserId { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime ExpireDate { get; set; }
    public UserInfoDTO UserInfo { get; set; }
}
