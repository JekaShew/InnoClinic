using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.UserValidator
{
    public class ChangeUserStatusOfUserCommandValidator : AbstractValidator<ChangeUserStatusOfUserCommand>
    {
        public ChangeUserStatusOfUserCommandValidator()
        {
            RuleFor(c => c.UserIdUserStatusIdPairDTO.UserId)
                .NotEmpty()
                .NotNull()
                .WithMessage("The User shouldn't be Null!");

            RuleFor(c => c.UserIdUserStatusIdPairDTO.UserStatusId)
                .NotEmpty()
                .NotNull()
                .WithMessage("The User Status shouldn't be Null!");
        }
    }
}
