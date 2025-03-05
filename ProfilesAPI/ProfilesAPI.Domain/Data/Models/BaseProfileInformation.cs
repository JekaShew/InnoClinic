namespace ProfilesAPI.Domain.Data.Models;

public class BaseProfileInformation
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }
    public string? Address { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public string Photo { get; set; }
}
