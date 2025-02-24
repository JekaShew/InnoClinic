using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.UserValidators;

public class OldNewPassworPairDTOValidator : AbstractValidator<OldNewPasswordPairDTO>
{
    public OldNewPassworPairDTOValidator()
    {
        RuleFor(c => c.OldPassword)
            .NotEmpty()
            .NotNull()
            .WithMessage("User's Password shouldn't be Null!");

        RuleFor(c => c.NewPassword)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5)
            .WithMessage("The User's Password is required and should be at least 5 symbols long!");
    }
}
