using AuthorizationAPI.Application.CQS.Commands.RefreshTokenCommands;
using FluentValidation;

namespace AuthorizationAPI.Application.Validators.RefreshTokenValidators
{
    public class AddRefreshTokenCommandValidator : AbstractValidator<AddRefreshTokenCommand>
    {
        public AddRefreshTokenCommandValidator()
        {
            RuleFor(c => c.RefreshTokenDTO.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("The Refresh Token Id shouldn't be Null!");

            RuleFor(c => c.RefreshTokenDTO.IsRevoked)
                .NotNull()
                .WithMessage("The Refresh Token's Revoke status shouldn't be Null!");

            RuleFor(c => c.RefreshTokenDTO.ExpireDate)
                .NotNull()
                .WithMessage("The Refresh Token's Expire Date shouldn't be Null!");

            RuleFor(c => c.RefreshTokenDTO.UserId)
                .NotNull()
                .WithMessage("The Refresh Token must have User!");
        }
    }
}
