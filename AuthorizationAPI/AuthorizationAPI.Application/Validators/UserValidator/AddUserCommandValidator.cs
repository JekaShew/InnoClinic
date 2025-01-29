using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.UserValidator
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(c => c.RegistrationInfoDTO.FIO)
                .NotEmpty()
                .NotNull()
                .WithMessage("The User's FIO shouldn't be Null!");

            RuleFor(c => c.RegistrationInfoDTO.FIO)
                .MinimumLength(2)
                .WithMessage("The User FIO should be at least 2 symbols long!");

            RuleFor(c => c.RegistrationInfoDTO.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("Email is required!");

            RuleFor(c => c.RegistrationInfoDTO.Email)
                .EmailAddress()
                .WithMessage("Email Address should be valid!");

            RuleFor(c => c.RegistrationInfoDTO.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("User's Password shouldn't be Null!");


            RuleFor(c => c.RegistrationInfoDTO.Password)
                .MinimumLength(5)
                .WithMessage("The User's Password should be at least 5 symbols long!");

            RuleFor(c => c.RegistrationInfoDTO.SecretPhrase)
               .NotEmpty()
               .NotNull()
               .WithMessage("User's Secret Word shouldn't be Null!");


            RuleFor(c => c.RegistrationInfoDTO.SecretPhrase)
                .MinimumLength(5)
                .WithMessage("The User's Secret Word should be at least 5 symbols long!");
        }
    }
}
