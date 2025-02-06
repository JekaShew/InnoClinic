using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.RoleValidators;

public class RoleForUpdateDTOValidator : AbstractValidator<RoleForUpdateDTO>
{
    public RoleForUpdateDTOValidator()
    {
        RuleFor(r => r.Title)
          .NotEmpty()
          .NotNull()
          .MinimumLength(2)
          .WithMessage("The Role's Title is required and should be at least 2 symbols long!");
    }
}
