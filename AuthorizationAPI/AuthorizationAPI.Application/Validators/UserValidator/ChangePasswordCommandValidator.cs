using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.UserValidator
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull()
                .WithMessage("The User shouldn't be Null!");

            RuleFor(c => c.NewPassword)
               .NotEmpty()
               .NotNull()
               .WithMessage("User's Password shouldn't be Null!");


            RuleFor(c => c.NewPassword)
                .MinimumLength(5)
                .WithMessage("The User's Password should be at least 5 symbols long!");
        }
    }
}
