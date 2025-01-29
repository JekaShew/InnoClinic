using AuthorizationAPI.Application.CQS.Commands.RoleCommands;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.RoleValidators
{
    public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
    {
        public AddRoleCommandValidator()
        {
            RuleFor(c => c.RoleDTO.Title)
               .NotEmpty()
               .NotNull()
               .WithMessage("The Role should have Title!");

            RuleFor(c => c.RoleDTO.Title)
                .MinimumLength(2)
                .WithMessage("The Role's Title should be at least 2 symbols long!");
        }
    }
}
