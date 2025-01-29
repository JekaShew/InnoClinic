using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.UserValidator
{
    public class CheckEmailPasswordPairQueryValidator : AbstractValidator<CheckEmailPasswordPairQuery>
    {
        public CheckEmailPasswordPairQueryValidator()
        {
            RuleFor(c => c.LoginInfoDTO.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("The Email shouldn't be Null!");

            RuleFor(c => c.LoginInfoDTO.Email)
                .EmailAddress()
                .WithMessage("Email Address should be valid!");

            RuleFor(c => c.LoginInfoDTO.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("User's Password shouldn't be Null!");


            RuleFor(c => c.LoginInfoDTO.Password)
                .MinimumLength(5)
                .WithMessage("The User's Password should be at least 5 symbols long!");

        }
    }
}
