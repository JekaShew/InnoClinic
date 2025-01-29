using AuthorizationAPI.Application.CQS.Commands.RoleCommands;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.RoleValidators
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(c => c.RoleDTO.Title)
               .NotEmpty()
               .NotNull()
               .WithMessage("The Role must have Title!");

            RuleFor(c => c.RoleDTO.Title)
                .MinimumLength(2)
                .WithMessage("The Role's Title should be at least 2 symbols long!");
        }
    }
}
