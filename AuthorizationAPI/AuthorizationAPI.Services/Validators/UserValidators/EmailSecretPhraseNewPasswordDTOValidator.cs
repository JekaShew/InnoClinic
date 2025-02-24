using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.UserValidators;

public class EmailSecretPhraseNewPasswordDTOValidator : AbstractValidator<EmailSecretPhraseNewPasswordDTO>
{
    public EmailSecretPhraseNewPasswordDTOValidator()
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

        RuleFor(c => c.NewPassword)
          .NotEmpty()
          .NotNull()
          .MinimumLength(5)
          .WithMessage("The User's Password is required and should be at least 5 symbols long!");
    }
}
