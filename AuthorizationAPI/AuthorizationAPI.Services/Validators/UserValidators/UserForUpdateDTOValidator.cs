using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.UserValidators;

public class UserForUpdateDTOValidator : AbstractValidator<UserForUpdateDTO>
{
    public UserForUpdateDTOValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .WithMessage("The User's First Name should be at least 2 symbols long!");

        RuleFor(c => c.LastName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .WithMessage("The User's Last Name should be at least 2 symbols long!");
    }
}
