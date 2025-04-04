using FluentValidation;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.Validators.SpecializationValidators;

public class SpecializationForUpdateDTOValidator : AbstractValidator<SpecializationForUpdateDTO>
{
    public SpecializationForUpdateDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull()
            .WithMessage("Specialization's Title is required!")
            .MaximumLength(60)
            .WithMessage("Specialization's Title should be less than 60 symbols!");
    }
}
