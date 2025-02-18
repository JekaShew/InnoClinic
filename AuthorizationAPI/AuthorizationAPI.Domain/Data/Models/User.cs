using Riok.Mapperly.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationAPI.Domain.Data.Models;

public class User
{
    public User()
    {
        RegistrationDate = DateTime.UtcNow;
    }
    public Guid Id { get; set; }
    [Required]
    [MinLength(2)]
    public string FirstName { get; set; }
    [Required]
    [MinLength(2)]
    public string LastName { get; set; }
    public string? SecondName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string? Phone { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    public string SecurityStamp { get; set; }
    [Required]
    public string SecretPhraseHash { get; set; }
    [Required]
    public DateTime RegistrationDate { get; set; }
    [Required]
    public Guid UserStatusId { get; set; }
    public UserStatus UserStatus { get; set; }
    [Required]
    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    public ICollection<RefreshToken>? RefreshTokens { get; set; }

}
