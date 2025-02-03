using System.ComponentModel.DataAnnotations;

namespace AuthorizationAPI.Domain.Data.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(3), MaxLength(60)]
        public string Title { get; set; }
        public string? Description { get; set; }

        public ICollection<User> Users { get; set; }

    }
}
