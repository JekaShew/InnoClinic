using FluentValidation;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Validators.ReceptionistValidators;

public class ReceptionistParametersValidator : AbstractValidator<ReceptionistParameters>
{
    public ReceptionistParametersValidator()
    {
        RuleFor(x => x.Offices)
           .Must(offices => offices == null
            || offices.All(office => office is string))
           .WithMessage("Incorrect Office value!");
    }
}
