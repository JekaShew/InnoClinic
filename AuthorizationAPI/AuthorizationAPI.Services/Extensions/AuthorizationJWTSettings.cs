using System.ComponentModel.DataAnnotations;

namespace AuthorizationAPI.Services.Extensions;

public class AuthorizationJWTSettings
{
    public const string ConfigurationSection = "Authorization";
    [Required]
    public string SecretKey { get; set; }
    [Required]
    public string Issuer { get; set; }
    [Required]
    public string Audience { get; set; }
}
