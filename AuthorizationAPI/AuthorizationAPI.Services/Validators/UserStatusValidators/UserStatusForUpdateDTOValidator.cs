using AuthorizationAPI.Shared.DTOs.UserStatusDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.UserStatusValidators;

public class UserStatusForUpdateDTOValidator : AbstractValidator<UserStatusForUpdateDTO>
{
    public UserStatusForUpdateDTOValidator()
    {
        RuleFor(us => us.Title)
        .NotEmpty()
        .NotNull()
        .MinimumLength(2)
        .MaximumLength(60)
        .WithMessage("The User Status's Title is required and should be at least 2 symbols long!");
    }
}
