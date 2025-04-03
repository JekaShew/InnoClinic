using AuthorizationAPI.Shared.DTOs.UserDTOs;

namespace AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs;

public class RefreshTokenInfoDTO
{
    public Guid Id { get; set; }    
    public bool IsRevoked { get; set; }
    public DateTime ExpireDate { get; set; }
    public Guid UserId { get; set; }
    public UserInfoDTO User { get; set; }
}
