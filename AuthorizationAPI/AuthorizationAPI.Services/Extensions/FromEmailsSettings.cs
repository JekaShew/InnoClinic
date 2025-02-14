using System.ComponentModel.DataAnnotations;

namespace AuthorizationAPI.Services.Extensions
{
    public class FromEmailsSettings
    {
        public const string ConfigurationSection = "EmailSettings:FromEmails";
        [Required]
        public string Default { get; set; }
        public string Doctor{ get; set; }
        public string Administrator { get; set; }
    }
}
