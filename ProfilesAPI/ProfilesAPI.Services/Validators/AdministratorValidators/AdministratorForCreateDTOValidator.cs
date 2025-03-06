using FluentValidation;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Services.Validators.AdministratorValidators;

public class AdministratorForCreateDTOValidator : AbstractValidator<AdministratorForCreateDTO>
{
    public AdministratorForCreateDTOValidator()
    {
        
    }
}
