using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.UserValidators;

public class EmailPasswordPairDTOValidator : AbstractValidator<EmailPasswordPairDTO>
{
    public EmailPasswordPairDTOValidator()
    {
        RuleFor(c => c.NewEmail)
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
