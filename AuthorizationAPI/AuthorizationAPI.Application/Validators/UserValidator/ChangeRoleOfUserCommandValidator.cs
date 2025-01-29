using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.UserValidator
{
    public class ChangeRoleOfUserCommandValidator : AbstractValidator<ChangeRoleOfUserCommand>
    {
        public ChangeRoleOfUserCommandValidator()
        {
            RuleFor(c => c.UserIdRoleIdPairDTO.UserId)
               .NotEmpty()
               .NotNull()
               .WithMessage("The User shouldn't be Null!");

            RuleFor(c => c.UserIdRoleIdPairDTO.RoleId)
                .NotEmpty()
                .NotNull()
                .WithMessage("The Role shouldn't be Null!");
        }
    }
}
