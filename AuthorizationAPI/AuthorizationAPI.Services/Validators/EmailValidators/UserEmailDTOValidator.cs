using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.EmailValidators
{
    public class UserEmailDTOValidator : AbstractValidator<UserEmailDTO>
    {
        public UserEmailDTOValidator()
        {
            RuleFor(e => e.Subject)
                .NotEmpty()
                .NotNull()
                .EmailAddress()
                .WithMessage("Recipient's Email is required and should be valid!");

            RuleFor(e => e.Subject)
                .NotEmpty()
                .NotNull()
                .WithMessage("Letter's Subject shouldn't be null!");

            RuleFor(e => e.Body)
                .NotEmpty()
                .NotNull()
                .WithMessage("Empty messages are Forbidden!!");
        }
    }
}
