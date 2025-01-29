using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.UserValidator
{
    public class TakeAuthorizationInfoDTOByEmailQueryValidator : AbstractValidator<TakeAuthorizationInfoDTOByEmailQuery>
    {
        public TakeAuthorizationInfoDTOByEmailQueryValidator()
        {
            RuleFor(c => c.Email)
               .NotEmpty()
               .NotNull()
               .WithMessage("The Email shouldn't be Null!");

            RuleFor(c => c.Email)
                .EmailAddress()
                .WithMessage("Email Address should be valid!");
        }
    }
}
