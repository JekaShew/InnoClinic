namespace AuthorizationAPI.Shared.DTOs.UserDTOs;

public class RegistrationInfoDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SecondName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public string SecretPhrase { get; set; }
}
