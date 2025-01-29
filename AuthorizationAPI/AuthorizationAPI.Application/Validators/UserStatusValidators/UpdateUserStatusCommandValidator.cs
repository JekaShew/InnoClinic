using AuthorizationAPI.Application.CQS.Commands.UserStatusCommands;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.UserStatusValidators
{
    public class UpdateUserStatusCommandValidator : AbstractValidator<UpdateUserStatusCommand>
    {
        public UpdateUserStatusCommandValidator()
        {
            RuleFor(c => c.UserStatusDTO.Title)
                .NotEmpty()
                .NotNull()
                .WithMessage("The User Status should have Title!");

            RuleFor(c => c.UserStatusDTO.Title)
                .MinimumLength(2)
                .WithMessage("The User Status's Title should be at least 2 symbols long!");
        }
    }
}
