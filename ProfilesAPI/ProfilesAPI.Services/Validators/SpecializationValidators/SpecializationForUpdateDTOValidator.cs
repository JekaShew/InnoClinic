using FluentValidation;
using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Services.Validators.SpecializationValidators;

public class SpecializationForUpdateDTOValidator : AbstractValidator<SpecializationForUpdateDTO>
{
    public SpecializationForUpdateDTOValidator()
    {
        RuleFor(c => c.Title)
           .NotEmpty()
           .NotNull()
           .WithMessage("Specialization's title shouldn't be null!");
    }
}
