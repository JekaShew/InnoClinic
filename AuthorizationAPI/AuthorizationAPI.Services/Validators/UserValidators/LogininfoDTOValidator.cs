using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.UserValidators;

public class LogininfoDTOValidator : AbstractValidator<LoginInfoDTO>
{
    public LogininfoDTOValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress()
            .WithMessage("Email Address should be valid!");

        RuleFor(c => c.Password)
            .NotEmpty()
            .NotNull()
            .WithMessage("User's Password shouldn't be Null!");
    }
}
