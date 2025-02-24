using AuthorizationAPI.Shared.DTOs.RoleDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.RoleValidators;

public class RoleForCreateDTOValidator : AbstractValidator<RoleForCreateDTO>
{
    public RoleForCreateDTOValidator()
    {
        RuleFor(r => r.Title)
          .NotEmpty()
          .NotNull()
          .MinimumLength(2)
          .MaximumLength(60)
          .WithMessage("The Role's Title is required and should be at least 2 symbols long!");
    }
}
