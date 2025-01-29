using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.UserValidator
{
    public class IsEmailRegisteredQueryValidator : AbstractValidator<IsEmailRegisteredQuery>
    {
        public IsEmailRegisteredQueryValidator()
        {
            RuleFor(c => c.EnteredEmail)
              .NotEmpty()
              .NotNull()
              .WithMessage("The Email shouldn't be Null!");

            RuleFor(c => c.EnteredEmail)
                .EmailAddress()
                .WithMessage("Email Address should be valid!");
        }
    }
}
