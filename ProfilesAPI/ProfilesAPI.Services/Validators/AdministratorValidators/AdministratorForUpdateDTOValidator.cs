using FluentValidation;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Services.Validators.AdministratorValidators;

public class AdministratorForUpdateDTOValidator : AbstractValidator<AdministratorForUpdateDTO>
{
    public AdministratorForUpdateDTOValidator()
    {
        
    }
}
