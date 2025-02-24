using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;

namespace AuthorizationAPI.Services.Validators.UserValidators;

public class UserForUpdateByAdministratorDTOValidator : AbstractValidator<UserForUpdateByAdministratorDTO>
{
    public UserForUpdateByAdministratorDTOValidator()
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

        RuleFor(c => c.RoleId)
             .NotEmpty()
             .NotNull()
             .WithMessage("Role is required!");

        RuleFor(c => c.UserStatusId)
            .NotEmpty()
            .NotNull()
            .WithMessage("User Status is required!");
    }
}
