using FluentValidation;
using ProfilesAPI.Shared.DTOs.WorkStatusDTOs;

namespace ProfilesAPI.Services.Validators.WorkStatusValidators;

public class WorkStatusForUpdateDTOValidator : AbstractValidator<WorkStatusForUpdateDTO>
{
    public WorkStatusForUpdateDTOValidator()
    {
        RuleFor(c => c.Title)
       .NotEmpty()
       .NotNull()
       .WithMessage("Work Status's title shouldn't be null!");
    }
}
