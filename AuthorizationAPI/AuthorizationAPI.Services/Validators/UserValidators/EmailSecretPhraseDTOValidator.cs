using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.UserValidators;

public class EmailSecretPhraseDTOValidator : AbstractValidator<EmailSecretPhrasePairDTO>
{
    public EmailSecretPhraseDTOValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress()
            .WithMessage("Email Address should be valid!");

        RuleFor(c => c.SecretPhrase)
          .NotEmpty()
          .NotNull()
          .WithMessage("User's Secret Phrase shouldn't be Null!");
    }
}
