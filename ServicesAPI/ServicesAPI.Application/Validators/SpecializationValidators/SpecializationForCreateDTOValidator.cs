using FluentValidation;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.Validators.SpecializationValidators;

internal class SpecializationForCreateDTOValidator : AbstractValidator<SpecializationForCreateDTO>
{
    public SpecializationForCreateDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull()
            .WithMessage("Title is required!")
            .MaximumLength(60)
            .WithMessage("Title should be less than 60 symbols!");
    }
}

