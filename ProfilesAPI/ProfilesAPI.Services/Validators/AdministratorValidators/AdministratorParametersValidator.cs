using FluentValidation;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Services.Validators.AdministratorValidators;

public class AdministratorParametersValidator : AbstractValidator<AdministratorParameters>
{
    public AdministratorParametersValidator()
    {
        RuleFor(x => x.Offices)
           .Must(offices => offices == null 
            || offices.All(office => office is string))
           .WithMessage("Incorrect Office value!");
    }
}
