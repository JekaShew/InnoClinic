using System.ComponentModel.DataAnnotations;

namespace AuthorizationAPI.Domain.Data.Models;

public class UserStatus
{
    public Guid Id { get; set; }
    [Required]
    [MinLength(2)]
    public string Title { get; set; }
    public string? Description { get; set; }

    public ICollection<User> Users { get; set; }
}
