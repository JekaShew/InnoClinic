using FluentValidation;
using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Services.Validators.SpecializationValidators;

public class SpecializationForCreateDTOValidator : AbstractValidator<SpecializationForCreateDTO>
{
    public SpecializationForCreateDTOValidator()
    {
        RuleFor(c => c.Title)
           .NotEmpty()
           .NotNull()
           .WithMessage("Specialization's title shouldn't be null!");
    }
}
