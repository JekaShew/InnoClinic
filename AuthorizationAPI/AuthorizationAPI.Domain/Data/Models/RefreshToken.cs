using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationAPI.Domain.Data.Models;

public class RefreshToken
{
    public Guid Id { get; set; }
    [Required]
    [DefaultValue(false)]
    public bool IsRevoked { get; set; }
    [Required]
    public DateTime ExpireDate { get; set; }
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; }
}
