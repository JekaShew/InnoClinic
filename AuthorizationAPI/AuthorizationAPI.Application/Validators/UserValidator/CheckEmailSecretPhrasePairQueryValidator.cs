using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.UserValidator
{
    public class CheckEmailSecretPhrasePairQueryValidator : AbstractValidator<CheckEmailSecretPhrasePairQuery>
    {
        public CheckEmailSecretPhrasePairQueryValidator()
        {
            RuleFor(c => c.EmailSecretPhrasePairDTO.Email)
              .NotEmpty()
              .NotNull()
              .WithMessage("The Email shouldn't be Null!");

            RuleFor(c => c.EmailSecretPhrasePairDTO.Email)
                .EmailAddress()
                .WithMessage("Email Address should be valid!");

            RuleFor(c => c.EmailSecretPhrasePairDTO.SecretPhrase)
              .NotEmpty()
              .NotNull()
              .WithMessage("User's Secret Phrase shouldn't be Null!");


            RuleFor(c => c.EmailSecretPhrasePairDTO.SecretPhrase)
                .MinimumLength(5)
                .WithMessage("The User's Secret Phrase should be at least 5 symbols long!");
        }
    }
}
