using System.ComponentModel.DataAnnotations;

namespace ProfilesAPI.Services.Extensions;

public class BlobContainerTitles
{
    public const string ConfigurationSection = "BlobStorage";
    [Required]
    public string ContainerTitle { get; set; }
}
