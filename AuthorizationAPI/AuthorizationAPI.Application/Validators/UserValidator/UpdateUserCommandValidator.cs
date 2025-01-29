using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.UserValidator
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(c => c.UserDTO.FIO)
               .NotEmpty()
               .NotNull()
               .WithMessage("The User's FIO shouldn't be Null!");

            RuleFor(c => c.UserDTO.FIO)
                .MinimumLength(2)
                .WithMessage("The User FIO should be at least 2 symbols long!");

            RuleFor(c => c.UserDTO.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("Email is required!");

            RuleFor(c => c.UserDTO.Email)
                .EmailAddress()
                .WithMessage("Email Address should be valid!");

            RuleFor(c => c.UserDTO.UserStatusId)
               .NotEmpty()
               .NotNull()
               .WithMessage("User's Status shouldn't be Null!");

            RuleFor(c => c.UserDTO.RoleId)
                .NotEmpty()
                .NotNull()
                .WithMessage("User's Role shouldn't be Null!");
        }
    }
}
