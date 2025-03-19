using FluentValidation;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Services.Validators.AdministratorValidators;

public class AdministratorParametersValidator : AbstractValidator<AdministratorParameters>
{
    public AdministratorParametersValidator()
    {
         
    }
}
