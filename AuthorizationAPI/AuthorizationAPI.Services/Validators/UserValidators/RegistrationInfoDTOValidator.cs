using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.UserValidators;

public class RegistrationInfoDTOValidator : AbstractValidator<RegistrationInfoDTO>
{
    public RegistrationInfoDTOValidator()
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

        RuleFor(c => c.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress()
            .WithMessage("Email Address is required and should be valid!");

        RuleFor(c => c.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5)
            .WithMessage("The User's Password is required and should be at least 5 symbols long!");

        RuleFor(c => c.SecretPhrase)
           .NotEmpty()
           .NotNull()
           .MinimumLength(5)
           .WithMessage("The User's Secret Word is required should be at least 5 symbols long!");
    }
}
