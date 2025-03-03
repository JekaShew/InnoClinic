using System.ComponentModel.DataAnnotations;

namespace OfficesAPI.Persistance.Extensions;

public class ConnectionStringsSettings
{
    public const string ConfigurationSection = "ConnectionStrings";
    [Required]
    public string OfficesDB { get; set; }
}
